using System.Linq;
using System.Net;
using System.Web.Mvc;
using Tienda.Helpers;
using Tienda.Models;

namespace Tienda.Controllers
{
    [Authorize(Roles = Rol.Admin + "," + Rol.Vendedor)]
    public class ProveedoresController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Index()
        {
            var _proveedores = _context.Proveedores.ToList();

            return View(_proveedores);
        }

        public ActionResult Detalles(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);

            var _proveedor = _context.Proveedores.SingleOrDefault(x => x.Id == id);

            if (_proveedor == null)
                return HttpNotFound();


            return View(_proveedor);
        }

        public ActionResult Crud(int id = 0)
        {
            if (id > 0)
            {
                var _proveedorInDb = _context.Proveedores.SingleOrDefault(x => x.Id == id);

                if (_proveedorInDb == null)
                    return HttpNotFound();

                return View(_proveedorInDb);
            }

            return View();
        }

        public ActionResult Guardar(Proveedor proveedor)
        {
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {



                if (proveedor.Id == 0)
                {
                    _context.Proveedores.Add(proveedor);
                }
                else
                {
                    var _proveedorInDB = _context.Proveedores.SingleOrDefault(x => x.Id == proveedor.Id);

                    _proveedorInDB.Rut = proveedor.Rut;
                    _proveedorInDB.Nombre = proveedor.Nombre;
                    _proveedorInDB.Correo = proveedor.Correo;
                    _proveedorInDB.Telefono = proveedor.Telefono;
                }

                if (IndexValidacion.SaveChanges(_context).Response)
                {
                    return RedirectToAction("Index", "Proveedores");
                }
                ModelState.AddModelError(string.Empty, "El rut ingresado ya se encuentra Registrado");

            }
            return View("Crud", proveedor);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();

            base.Dispose(disposing);
        }
    }
}