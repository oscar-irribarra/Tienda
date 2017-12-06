using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tienda.Models;

namespace Tienda.Controllers
{
    [Authorize(Roles = Rol.Admin + "," + Rol.Vendedor)]
    public class ImpuestosController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Index()
        {
            var impuestos = _context.Impuestos.ToList();

            return View(impuestos);
        }

        public ActionResult Crud(int id = 0 )
        {
            if (id > 0)
            {              
                var _impuestoInDb = _context.Impuestos.SingleOrDefault(x => x.Id == id);

                if (_impuestoInDb == null)
                    return HttpNotFound();

                return View(_impuestoInDb);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Guardar(Impuesto impuesto)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
                return View("Crud",impuesto);

            if (impuesto.Id == 0)
            {
                _context.Impuestos.Add(impuesto);
            }
            else
            {
                var _impuestoInDb = _context.Impuestos.SingleOrDefault(x => x.Id == impuesto.Id);

                _impuestoInDb.Nombre = impuesto.Nombre;
                _impuestoInDb.Tasa = impuesto.Tasa;
            }            
            _context.SaveChanges();

            return RedirectToAction("Index","Impuestos");
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
                _context.Dispose();

            base.Dispose(disposing);
        }

    }
}