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
    [Authorize(Roles = Rol.Admin + "," + Rol.Vendedor)]
    public class AdquisicionesController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        public ActionResult Index()
        {
            var _adquisiciones = _context.Adquisiciones.Include(x => x.Proveedor).Include(x => x.Vendedor).Include(x => x.Documento).ToList();
            return View(_adquisiciones);
        }

        public ActionResult Detalles(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);

            var _detalleadq = _context.Adquisiciones.SingleOrDefault(x => x.Id == id);

            if (_detalleadq == null)
                return HttpNotFound();

            return View(_detalleadq);
        }

        public ActionResult Crud()
        {
            var _listaproductos = (List<AgregarProductoView>)Session["CartAdquisicion"];
            if (_listaproductos == null)
                return RedirectToAction("PuntodeAdquisicion", "Adquisiciones");


            ViewData["DocumentoId"] = new SelectList(_context.Documentos.ToList(), "Id", "Nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guardar(AdquisionViewModel adqView)
        {
            if (!ModelState.IsValid)
                return View("Crud", adqView);

            var _consultaProveedores = _context.Proveedores.Where(x => x.Rut == adqView.Rut).SingleOrDefault();

            if (_consultaProveedores == null)
            {
                ModelState.AddModelError("", "Este proveedor no se encuentra registrado");
                return View("Crud", adqView);
            }

            var _cestaProductos = (List<AgregarProductoView>)Session["CartAdquisicion"];

            var _TipoProducto = _cestaProductos.First().IdTipoProducto;

            if (User.Identity.GetUserId() == null)
                return View("Crud", adqView);

            var _nuevaAdquisicion = new Adquisicion()
            {
                ProveedorId = _consultaProveedores.Id,
                DocumentoId = IdTipoDocumento.Factura,
                Fecha = DateTime.Now,
                EstadoId = Estados.Finalizada,
                EmpresaId = Empresas.Sostel,
                TipoProductoId = _TipoProducto,
                VendedorId = User.Identity.GetUserId()
            };
            _context.Adquisiciones.Add(_nuevaAdquisicion);

            foreach (var item in _cestaProductos)
            {
                var _nuevoDetalle = new DetalleAdquisicion()
                {
                    AdquisicionId = _nuevaAdquisicion.Id,
                    ProductoId = item.IdProducto,
                    Cantidad = item.Cantidad,
                    Precio = item.Precio,

                };
                var _inventario = _context.Inventarios.Where(x => x.ProductoId == item.IdProducto).FirstOrDefault();
                _inventario.Stock += item.Cantidad;

                _context.DetalleAdquisiciones.Add(_nuevoDetalle);
            }
            Session["CartAdquisicion"] = null;
            Session["ConteoAdquisicion"] = null;
            _context.SaveChanges();

            return RedirectToAction("Index", "Adquisiciones");
        }

        public ActionResult PuntodeAdquisicion()
        {
            var view = new AgregarProductoView
            {
                DetalleCart = (List<AgregarProductoView>)Session["CartAdquisicion"],
            };
            return View(view);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PuntodeAdquisicion(AgregarProductoView aPview)
        {
            var _consultaProducto = _context.Productos.Where(p => p.Barcode == aPview.Barcode).SingleOrDefault();

            if (_consultaProducto != null)
            {
                if (Session["CartAdquisicion"] == null)
                {
                    List<AgregarProductoView> _cestaProductos = new List<AgregarProductoView>();
                    ActualizarCesta(aPview, _consultaProducto, _cestaProductos);
                }
                else
                {
                    List<AgregarProductoView> _cestaProductos = (List<AgregarProductoView>)Session["CartAdquisicion"];
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
                                    item.Cantidad += aPview.Cantidad;
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
                ModelState.AddModelError(string.Empty, "El codigo ingresado no existe");
            }
            aPview.DetalleCart = (List<AgregarProductoView>)Session["CartAdquisicion"];
            return View(aPview);
        }

        private void ActualizarCesta(AgregarProductoView aPview, Producto _consultaProducto, List<AgregarProductoView> _cestaProductos)
        {
            aPview.IdProducto = _consultaProducto.Id;
            aPview.Nombre = _consultaProducto.Nombre;
            aPview.Precio = aPview.Precio;
            aPview.IdTipoProducto = _consultaProducto.TipoProductoId;
            _cestaProductos.Add(aPview);

            Session["CartAdquisicion"] = _cestaProductos;
            Session["ConteoAdquisicion"] = Convert.ToInt32(Session["ConteoAdquisicion"]) + 1;
        }

        public ActionResult EliminarProducto(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var _cestaProductos = (List<AgregarProductoView>)Session["CartAdquisicion"];
            var _productoInCesta = _cestaProductos.Where(l => l.IdProducto == id).SingleOrDefault();

            if (_productoInCesta == null || _cestaProductos == null)
                return HttpNotFound();

            _cestaProductos.RemoveAll(x => x.IdProducto == id);
            Session["ConteoAdquisicion"] = Convert.ToInt32(Session["ConteoAdquisicion"]) - 1;

            if (_cestaProductos.Count == 0)
                Session["CartAdquisicion"] = null;

            return RedirectToAction("PuntodeAdquisicion");
        }

    }
}