using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Tienda.Helpers;
using Tienda.Models;
using Tienda.ViewModels;

namespace Tienda.Controllers
{
    [Authorize(Roles = Rol.Admin + "," + Rol.Vendedor)]
    public class ProductosController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Index()
        {
            var productos = _context.Productos.Include(p => p.Categoria).OrderBy(x => x.TipoProductoId).ToList();

            return View(productos);
        }

        public ActionResult Detalles(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);

            var producto = _context.Productos.Include(p => p.DetalleProducto).Include(p => p.Inventario).SingleOrDefault(p => p.Id == id);

            if (producto == null)
                return HttpNotFound();

            return View(producto);
        }

        public ActionResult Crud(int id = 0)
        {
            var _productoInDb = _context.Productos.SingleOrDefault(x => x.Id == id);
            var _detalleInDb = _context.DetalleProductos.SingleOrDefault(x => x.ProductoId == id);
            var productoView = new FormProductoViewModel() { DetalleProductofrm = null };
            if (id > 0)
            {
                if (_productoInDb == null)
                    return HttpNotFound();

                productoView.Productofrm = _productoInDb;
                if (_detalleInDb != null)
                    productoView.DetalleProductofrm = _detalleInDb;

                ViewData["Productofrm.CategoriaID"] = new SelectList(_context.Categorias.OrderBy(c => c.Nombre), "Id", "Nombre", _productoInDb.CategoriaId);
                ViewData["Productofrm.ImpuestoID"] = new SelectList(_context.Impuestos.OrderBy(i => i.Tasa), "Id", "Nombre", _productoInDb.ImpuestoId);
                ViewData["Productofrm.TipoProductoID"] = new SelectList(_context.TipoProductos.OrderBy(i => i.Id), "Id", "Nombre", _productoInDb.TipoProductoId);

                return View(productoView);
            }

            ViewData["Productofrm.CategoriaID"] = new SelectList(_context.Categorias.OrderBy(c => c.Nombre), "Id", "Nombre");
            ViewData["Productofrm.ImpuestoID"] = new SelectList(_context.Impuestos.OrderBy(i => i.Tasa), "Id", "Nombre");
            ViewData["Productofrm.TipoProductoID"] = new SelectList(_context.TipoProductos.OrderBy(i => i.Id), "Id", "Nombre");

            return View(productoView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guardar(FormProductoViewModel productoview)
        {
            ModelState.Remove("Productofrm.Id");
            ModelState.Remove("DetalleProductofrm.Id");
            if (ModelState.IsValid)
            {
                if (productoview.Productofrm.Id == 0)
                {
                    _context.Productos.Add(productoview.Productofrm);

                    var inventario = new Inventario { ProductoId = productoview.Productofrm.Id, Stock = productoview.Cantidad };
                    _context.Inventarios.Add(inventario);

                    if (productoview.Productofrm.TipoProductoId == Tipo_negocio.Seguridad)
                    {
                        productoview.DetalleProductofrm.ProductoId = productoview.Productofrm.Id;
                        _context.DetalleProductos.Add(productoview.DetalleProductofrm);
                        if (IndexValidacion.SaveChanges(_context).Response)
                        {
                            var detalleInBD = _context.DetalleProductos.SingleOrDefault(x => x.ProductoId == productoview.Productofrm.Id);
                            detalleInBD.ImagenFile = productoview.DetalleProductofrm.ImagenFile;
                            ImagenHelper.SubirImagen(detalleInBD);
                        }
                        else
                        {
                            productoview.DetalleProductofrm.ImagenFile = productoview.DetalleProductofrm.ImagenFile;
                            ModelState.AddModelError("Codigoproducto", IndexValidacion.SaveChanges(_context).Message);

                        }
                    }
                    if (IndexValidacion.SaveChanges(_context).Response)
                    {
                        return RedirectToAction("Index", "Productos");
                    }
                    ModelState.AddModelError("Codigoproducto", IndexValidacion.SaveChanges(_context).Message);
                }
                else
                {
                    var productoInBd = _context.Productos.Single(x => x.Id == productoview.Productofrm.Id);
                    var detalleInBD = _context.DetalleProductos.SingleOrDefault(x => x.ProductoId == productoview.Productofrm.Id);

                    productoInBd.Nombre = productoview.Productofrm.Nombre;
                    productoInBd.Barcode = productoview.Productofrm.Barcode;
                    productoInBd.CategoriaId = productoview.Productofrm.CategoriaId;
                    productoInBd.ImpuestoId = productoview.Productofrm.ImpuestoId;
                    productoInBd.Precio = productoview.Productofrm.Precio;
                    productoInBd.FechaVencimiento = productoview.Productofrm.FechaVencimiento;

                    if (detalleInBD != null)
                    {
                        detalleInBD.Marca = productoview.DetalleProductofrm.Marca;
                        detalleInBD.Color = productoview.DetalleProductofrm.Color;
                        detalleInBD.Descripcion = productoview.DetalleProductofrm.Descripcion;
                        detalleInBD.Modelo = productoview.DetalleProductofrm.Modelo;
                        detalleInBD.ImagenFile = productoview.DetalleProductofrm.ImagenFile;
                        ImagenHelper.SubirImagen(detalleInBD);
                    }

                    if (IndexValidacion.SaveChanges(_context).Response)
                    {
                        return RedirectToAction("Index", "Productos");
                    }
                    ModelState.AddModelError("Codigoproducto", IndexValidacion.SaveChanges(_context).Message);
                }
            }
            ViewData["Productofrm.CategoriaID"] = new SelectList(_context.Categorias.OrderBy(c => c.Nombre), "Id", "Nombre", productoview.Productofrm.CategoriaId);
            ViewData["Productofrm.ImpuestoID"] = new SelectList(_context.Impuestos.OrderBy(i => i.Tasa), "Id", "Nombre", productoview.Productofrm.ImpuestoId);
            ViewData["Productofrm.TipoProductoID"] = new SelectList(_context.TipoProductos.OrderBy(i => i.Id), "Id", "Nombre", productoview.Productofrm.TipoProductoId);
            return View("Crud", productoview);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();
            base.Dispose(disposing);
        }

    }
}