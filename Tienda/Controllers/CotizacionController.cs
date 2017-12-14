using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tienda.Models;

namespace Tienda.Controllers
{
    [Authorize(Roles = Rol.Vendedor)]
    public class CotizacionController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        [Authorize(Roles = Rol.Vendedor + "," + Rol.Admin)]

        public ActionResult Index()
        {
            var _cotizaciones = _context.Cotizaciones.ToList();

            return View(_cotizaciones);
        }

        [Authorize(Roles = Rol.Vendedor + "," + Rol.Admin)]
        public ActionResult Detalles(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadGateway);

            var _detallecotizaciones = _context.Cotizaciones.SingleOrDefault(x => x.Id == id);

            if (_detallecotizaciones == null)
                return HttpNotFound();

            return View(_detallecotizaciones);
        }
    }
}