using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tienda.Models;
using System.Data.Entity;
using Tienda.ViewModels;
using Microsoft.AspNet.Identity;

namespace Tienda.Controllers
{
    [Authorize(Roles = Rol.Vendedor)]
    public class VentasController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        [Authorize(Roles = Rol.Vendedor + "," + Rol.Admin)]

        public ActionResult Index()
        {
            var ventas = _context.Ventas.Include(x => x.Cliente).Include(x => x.Estado).OrderByDescending(x => x.Id).ToList();


            return View(ventas);
        }
        [Authorize(Roles = Rol.Vendedor + "," + Rol.Admin)]
        public ActionResult Detalles(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);

            var _detalleventas = _context.Ventas.SingleOrDefault(x => x.Id == id);

            if (_detalleventas == null)
                return HttpNotFound();

            return View(_detalleventas);
        }

        public ActionResult Crud()
        {
            var _listaproductos = (List<AgregarProductoView>)Session["CartVentas"];
            if (_listaproductos == null)
                return RedirectToAction("PuntodeVenta", "Ventas");

            ViewData["DocumentoId"] = new SelectList(_context.Documentos.ToList(), "Id", "Nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guardar(VentaViewModel ventaView)
        {
            if (!ModelState.IsValid)
                return View("Crud", ventaView);


            if (ventaView.Ispublica)
                ventaView.Rut = "0";
            
            var _consultaCliente = _context.Clientes.Where(x => x.Rut == ventaView.Rut).FirstOrDefault();

            if (_consultaCliente == null)
            {
                var _nuevocliente = new Cliente { Nombre = ventaView.Nombre, Apellido = ventaView.Apellido, Rut = ventaView.Rut, Telefono = ventaView.Telefono, Email = ventaView.Email };
                _context.Clientes.Add(_nuevocliente);

                _consultaCliente = _nuevocliente;
            }

            var _cestaProductos = (List<AgregarProductoView>)Session["CartVentas"];

            var _TipoProducto = _cestaProductos.First().IdTipoProducto;

            if (User.Identity.GetUserId() == null)
                return View("Crud", ventaView);

            var _nuevaVenta = new Venta()
            {
                ClienteId = _consultaCliente.Id,
                DocumentoId = IdTipoDocumento.Factura,
                Fecha = DateTime.Now,
                EstadoId = Estados.Finalizada,
                EmpresaId = Empresas.Sostel,
                EsOnline = false,
                TipoProductoId = _TipoProducto,
                VendedorId = User.Identity.GetUserId()
            };
            _context.Ventas.Add(_nuevaVenta);

            foreach (var item in _cestaProductos)
            {
                var _nuevoDetalle = new DetalleVenta()
                {
                    VentaId = _nuevaVenta.Id,
                    ProductoId = item.IdProducto,
                    Cantidad = item.Cantidad,
                    Precio = item.Precio
                };
                var _inventario = _context.Inventarios.Where(x => x.ProductoId == item.IdProducto).FirstOrDefault();
                _inventario.Stock -= item.Cantidad;

                _context.DetalleVentas.Add(_nuevoDetalle);
            }
            Session["CartVentas"] = null;
            Session["ConteoVentas"] = null;
            _context.SaveChanges();

            return RedirectToAction("Detalles/" + _nuevaVenta.Id, "Ventas");
        }

        public ActionResult PuntodeVenta()
        {
            var view = new AgregarProductoView
            {
                DetalleCart = (List<AgregarProductoView>)Session["CartVentas"],
                listaproductos = _context.Productos.ToList()
            };
            return View(view);
        }

        [HttpPost]
        public ActionResult PuntodeVenta(AgregarProductoView aPview)
        {
            aPview.Cantidad = 1;
            var _consultaProducto = _context.Productos.Where(p => p.Barcode == aPview.Barcode).SingleOrDefault();

            if (_consultaProducto != null)
            {
                if (!(aPview.Cantidad > _consultaProducto.Inventario.Single().Stock))
                {
                    if (Session["CartVentas"] == null)
                    {
                        List<AgregarProductoView> _cestaProductos = new List<AgregarProductoView>();
                        ActualizarCesta(aPview, _consultaProducto, _cestaProductos);
                    }
                    else
                    {
                        List<AgregarProductoView> _cestaProductos = (List<AgregarProductoView>)Session["CartVentas"];
                        var _idPFirstLista = _cestaProductos.FirstOrDefault().IdProducto;
                        var _cProducto = _context.Productos.Where(x => x.Id == _idPFirstLista).Single();

                        if (_cProducto.TipoProductoId == _consultaProducto.TipoProductoId) //Acteco Diferente
                        {
                            if (_cestaProductos.Where(l => l.Barcode == aPview.Barcode).Count() >= 1)
                            {
                                foreach (var item in _cestaProductos)
                                {
                                    if (item.Barcode == aPview.Barcode)
                                    {
                                        if ((item.Cantidad + aPview.Cantidad) <= _consultaProducto.Inventario.Single().Stock)
                                        {
                                            item.Cantidad += aPview.Cantidad;
                                        }
                                        else
                                        {
                                            ModelState.AddModelError(string.Empty, "No existe Stock Suficiente del producto: " + _consultaProducto.Nombre);

                                        }
                                    }
                                }
                            }
                            else
                            {
                                ActualizarCesta(aPview, _consultaProducto, _cestaProductos);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Solo puede Ingresar Productos de un mismo tipo");

                        }

                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No existe Stock Suficiente del producto: " + _consultaProducto.Nombre);

                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "El codigo ingresado no existe");

            }
            aPview.DetalleCart = (List<AgregarProductoView>)Session["CartVentas"];
            return View(aPview);
        }

        private void ActualizarCesta(AgregarProductoView aPview, Producto _consultaProducto, List<AgregarProductoView> _cestaProductos)
        {
            aPview.IdProducto = _consultaProducto.Id;
            aPview.Nombre = _consultaProducto.Nombre;
            aPview.Precio = _consultaProducto.Precio;
            aPview.IdTipoProducto = _consultaProducto.TipoProductoId;
            _cestaProductos.Add(aPview);

            Session["CartVentas"] = _cestaProductos;
            Session["ConteoVentas"] = Convert.ToInt32(Session["ConteoVentas"]) + 1;
        }

        public ActionResult ConteoCesta(int? barcode, int c)
        {
            List<AgregarProductoView> _cestaProductos = (List<AgregarProductoView>)Session["CartVentas"];

            var _consultaProducto = _context.Productos.SingleOrDefault(x => x.Barcode == barcode);

            if (_consultaProducto == null || barcode == null)
                return RedirectToAction("PuntodeVenta");

            if (_cestaProductos.Where(l => l.Barcode == barcode).Count() >= 1)
            {
                foreach (var item in _cestaProductos)
                {
                    if (item.Barcode == barcode)
                    {
                        if ((item.Cantidad + c) <= _consultaProducto.Inventario.Single().Stock && (item.Cantidad + c) > 0)
                        {
                            item.Cantidad += c;
                        }
                    }
                }
            }

            return RedirectToAction("PuntodeVenta");
        }

        public ActionResult EliminarProducto(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _cestaProductos = (List<AgregarProductoView>)Session["CartVentas"];
            var _productoInCesta = _cestaProductos.Where(l => l.IdProducto == id).SingleOrDefault();

            if (_productoInCesta == null || _cestaProductos == null)
                return HttpNotFound();

            _cestaProductos.RemoveAll(x => x.IdProducto == id);
            Session["ConteoVentas"] = Convert.ToInt32(Session["ConteoVentas"]) - 1;

            if (_cestaProductos.Count == 0)
                Session["CartVentas"] = null;

            return RedirectToAction("PuntodeVenta");
        }

        public ActionResult FinalizarVenta(int? id)
        {
            if (id == null)
                return HttpNotFound();

            var _venta = _context.Ventas.SingleOrDefault(x => x.Id == id);

            if (_venta == null)
                return HttpNotFound();

            _venta.EstadoId = Estados.Finalizada;

            _context.SaveChanges();

            return RedirectToAction("Detalles/" + id, "Ventas");

        }


    }
}