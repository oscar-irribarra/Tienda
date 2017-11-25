using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tienda.Models;
using System.Data.Entity;
using System.Net;
using Tienda.ViewModels;
using Microsoft.AspNet.Identity;

namespace Tienda.Controllers
{
    public class ArriendosController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public ActionResult Index()
        {
            var _arriendos = _context.Arriendos.Include(x => x.Cliente).Include(x => x.Estado).OrderByDescending(x => x.Id).ToList();
            return View(_arriendos);
        }

        public ActionResult Detalles(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);

            var _detallearriendos = _context.Arriendos.SingleOrDefault(x => x.Id == id);

            if (_detallearriendos == null)
                return HttpNotFound();

            return View(_detallearriendos);
        }

        public ActionResult Crud()
        {
            var _listaproductos = (List<AgregarProductoView>)Session["CartArriendos"];
            if (_listaproductos == null)
                return RedirectToAction("PuntodeArriendo", "Arriendos");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guardar(ArriendoViewModel arriendoView)
        {
            if (!ModelState.IsValid)
                return View("Crud", arriendoView);

            var _consultaCliente = _context.Clientes.Where(x => x.Rut == arriendoView.Rut).SingleOrDefault();

            if (_consultaCliente == null)
            {
                ModelState.AddModelError("", "Este cliente no se encuentra registrado");
                return View("Crud", arriendoView);
            }

            var _cestaProductos = (List<AgregarProductoView>)Session["CartArriendos"];

            var _TipoProducto = _cestaProductos.First().IdTipoProducto;

            if (User.Identity.GetUserId() == null)
                return View("Crud", arriendoView);

            var _nuevoArriendo = new Arriendo()
            {
                ClienteId = _consultaCliente.Id,
                DocumentoId = IdTipoDocumento.Factura,
                FechaInicio = arriendoView.FechaInicio,
                FechaFin = arriendoView.FechaFin,
                EstadoId = Estados.EnCurso,
                EmpresaId = Empresas.Sostel,
                TipoProductoId = _TipoProducto,
                VendedorId = User.Identity.GetUserId(),
            };
            _context.Arriendos.Add(_nuevoArriendo);


            foreach (var item in _cestaProductos)
            {
                var _nuevoDetalle = new DetalleArriendo()
                {
                    ArriendoId = _nuevoArriendo.Id,
                    ProductoId = item.IdProducto,
                    Cantidad = item.Cantidad,
                };
                var _inventario = _context.Inventarios.Where(x => x.ProductoId == item.IdProducto).FirstOrDefault();
                _inventario.Stock -= item.Cantidad;

                _context.DetalleArriendos.Add(_nuevoDetalle);
            }
            Session["CartArriendos"] = null;
            Session["ConteoArriendos"] = null;
            _context.SaveChanges();

            return RedirectToAction("Detalles/"+_nuevoArriendo.Id, "Arriendos");
        }

        public ActionResult PuntodeArriendo()
        {
            var view = new AgregarProductoView
            {
                DetalleCart = (List<AgregarProductoView>)Session["CartArriendos"],
            };
            return View(view);
        }

        [HttpPost]
        public ActionResult PuntodeArriendo(AgregarProductoView aPview)
        {
            aPview.Cantidad = 1;
            var _consultaProducto = _context.Productos.Where(p => p.Barcode == aPview.Barcode).SingleOrDefault();

            if (_consultaProducto != null)
            {
                if (!(aPview.Cantidad > _consultaProducto.Inventario.Single().Stock))
                {
                    if (Session["CartArriendos"] == null)
                    {
                        List<AgregarProductoView> _cestaProductos = new List<AgregarProductoView>();
                        ActualizarCesta(aPview, _consultaProducto, _cestaProductos);
                    }
                    else
                    {
                        List<AgregarProductoView> _cestaProductos = (List<AgregarProductoView>)Session["CartArriendos"];
                        var _idPFirstLista = _cestaProductos.FirstOrDefault().IdProducto;
                        var _cProducto = _context.Productos.Where(x => x.Id == _idPFirstLista).Single();

                        if (_cProducto.TipoProductoId == _consultaProducto.TipoProductoId)
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
            aPview.DetalleCart = (List<AgregarProductoView>)Session["CartArriendos"];
            return View(aPview);
        }

        private void ActualizarCesta(AgregarProductoView aPview, Producto _consultaProducto, List<AgregarProductoView> _cestaProductos)
        {
            aPview.IdProducto = _consultaProducto.Id;
            aPview.Nombre = _consultaProducto.Nombre;
            aPview.IdTipoProducto = _consultaProducto.TipoProductoId;
            _cestaProductos.Add(aPview);

            Session["CartArriendos"] = _cestaProductos;
            Session["ConteoArriendos"] = Convert.ToInt32(Session["ConteoArriendos"]) + 1;
        }

        public ActionResult ConteoCesta(int? barcode, int c)
        {
            List<AgregarProductoView> _cestaProductos = (List<AgregarProductoView>)Session["CartArriendos"];

            var _consultaProducto = _context.Productos.SingleOrDefault(x => x.Barcode == barcode);

            if (_consultaProducto == null || barcode == null)
                return RedirectToAction("PuntodeArriendo");

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
                        else
                        {
                            ModelState.AddModelError(string.Empty, "No existe Stock Suficiente del producto: " + _consultaProducto.Nombre);
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "No existe Stock Suficiente del producto: " + _consultaProducto.Nombre);
            }

            return RedirectToAction("PuntodeArriendo");
        }

        public ActionResult EliminarProducto(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _cestaProductos = (List<AgregarProductoView>)Session["CartArriendos"];
            var _productoInCesta = _cestaProductos.Where(l => l.IdProducto == id).SingleOrDefault();

            if (_productoInCesta == null || _cestaProductos == null)
                return HttpNotFound();

            _cestaProductos.RemoveAll(x => x.IdProducto == id);
            Session["ConteoArriendos"] = Convert.ToInt32(Session["ConteoArriendos"]) - 1;

            if (_cestaProductos.Count == 0)
                Session["CartArriendos"] = null;

            return RedirectToAction("PuntodeArriendo");
        }
    }
}