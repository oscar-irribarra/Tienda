using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Tienda.Models;

namespace Tienda.Controllers
{
    [Authorize(Roles = Rol.Admin + "," + Rol.Vendedor)]
    public class CategoriasController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Index()
        {
            var categorias = _context.Categorias.Include(c => c.TipoProducto).ToList();
            return View(categorias);
        }

        public ActionResult Crud(int id = 0)
        {
            if (id > 0)
            {
                var _categoriaInDb = _context.Categorias.SingleOrDefault(x => x.Id == id);

                if (_categoriaInDb == null)
                    return HttpNotFound();

                var categoriaView = new Categoria
                {
                    TipoProductoId = _categoriaInDb.TipoProductoId,
                    Nombre = _categoriaInDb.Nombre
                };

                ViewData["TipoProductoID"] = new SelectList(_context.TipoProductos.OrderBy(i => i.Id), "Id", "Nombre", _categoriaInDb.TipoProductoId);

                return View(_categoriaInDb);
            }

            ViewData["TipoProductoID"] = new SelectList(_context.TipoProductos.OrderBy(i => i.Id), "Id", "Nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guardar(Categoria categoria)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                ViewData["TipoProductoID"] = new SelectList(_context.TipoProductos.OrderBy(i => i.Id), "Id", "Nombre", categoria.TipoProductoId);

                return View("Crud", categoria);
            }

            if (categoria.Id == 0)
            {
                _context.Categorias.Add(categoria);
            }
            else
            {
                var _categoriaInDb = _context.Categorias.SingleOrDefault(x => x.Id == categoria.Id);
                _categoriaInDb.Nombre = categoria.Nombre;
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Categorias");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();

            base.Dispose(disposing);
        }

    }
}