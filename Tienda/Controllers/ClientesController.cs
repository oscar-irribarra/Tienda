using System.Linq;
using System.Web.Mvc;
using Tienda.Helpers;
using Tienda.Models;

namespace Tienda.Controllers
{
    [Authorize(Roles = Rol.Admin + "," + Rol.Vendedor)]
    public class ClientesController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Index()
        {
            var clientes = _context.Clientes.Where(x => x.Rut != "0").ToList();
            return View(clientes);
        }

        public ActionResult Crud(int id = 0)
        {
            if (id > 0)
            {
                var _clienteInDb = _context.Clientes.SingleOrDefault(x => x.Id == id);

                if (_clienteInDb == null)
                    return HttpNotFound();

                return View(_clienteInDb);
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guardar(Cliente cliente)
        {
            ModelState.Remove("Id");

            if (ModelState.IsValid)
            {


                if (cliente.Id == 0)
                {
                    _context.Clientes.Add(cliente);
                }
                else
                {
                    var clienteInDb = _context.Clientes.SingleOrDefault(c => c.Id == cliente.Id);

                    clienteInDb.Nombre = cliente.Nombre;
                    clienteInDb.Rut = cliente.Rut;
                    clienteInDb.Telefono = cliente.Telefono;
                    clienteInDb.Apellido = cliente.Apellido;
                    clienteInDb.Email = cliente.Email;
                }

                if (IndexValidacion.SaveChanges(_context).Response)
                {
                    return RedirectToAction("Index", "Clientes");
                }
                ModelState.AddModelError(string.Empty, "El rut ingresado ya se encuentra Registrado");

            }
            return View("Crud",cliente);

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();

            base.Dispose(disposing);
        }
    }
}