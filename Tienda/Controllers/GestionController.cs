using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tienda.Models;
using System.Data.Entity;
namespace Tienda.Controllers
{
    public class GestionController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        // GET: Gestion
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Detalles()
        {
            var _empresa = _context.Empresas.ToList();
            return View();
        }
        //[HttpPost]
        public ActionResult GuardarComentario([Bind(Include = "nombre,correo,telefono,mensaje")] string nombre, string correo, string telefono, string mensaje)
        {
            var productos = _context.Productos.Where(p => p.TipoProductoId == Tipo_negocio.Seguridad).ToList();
                 
            var contacto = new Contacto
                {
                    Nombre = nombre,
                    Correo = correo,
                    Telefono = telefono,
                    Contenido = mensaje,
                    EstadoId = Estados.EnCurso,
                    Ip = Request.ServerVariables["REMOTE_ADDR"],
                    Fecha = DateTime.Now,
                };

                _context.Contactos.Add(contacto);
                _context.SaveChanges();


                return RedirectToAction("Index","Inicio",productos);
            

        }
        public ActionResult Crud(int id = 0)
        {
            if (id == 0)
            {
                var _empresaInDb = _context.Empresas.Single();

                return View(_empresaInDb);
            }
            return View();
        }

        public ActionResult Guardar(Empresa empresa)
        {
            if (!ModelState.IsValid)
                return View("Crud", empresa);

            var _empresaInDb = _context.Empresas.Single();
            _empresaInDb.RazonSocial = empresa.RazonSocial;
            _empresaInDb.RepresentanteLegal = empresa.RepresentanteLegal;
            _empresaInDb.Telefono = empresa.Telefono;
            _empresaInDb.Giro = empresa.Giro;
            _empresaInDb.Email = empresa.Email;
            _empresaInDb.Direccion = empresa.Direccion;
            _empresaInDb.Comuna = empresa.Comuna;
            _empresaInDb.Ciudad = empresa.Ciudad;

            _context.SaveChanges();

            return RedirectToAction("Index","gestion");
        }
        [AllowAnonymous]

        public ActionResult Reportes()
        {
            return View();
        }

        public class chart
        {
            public string nombre { get; set; }
            public int entero1 { get; set; }
            public int entero2 { get; set; }
            public int entero3 { get; set; }
            public double valor { get; set; }
            public double decimal1 { get; set; }
            public double decimal2 { get; set; }
            public double decimal3 { get; set; }
            public string color { get; set; }
        }
        [AllowAnonymous]
        public JsonResult Get(int a)
        {
            List<chart> lista = new List<chart>();
            switch (a)
            {
                case 1:
                    VentasYarriendosTotales(lista);
                    break;
                case 2:
                    PorcentajeLocalVsOnline(lista);
                    break;
                case 3:
                    VentasVsArriendos(lista);
                    break;
                default:
                    PorcentajeLocalVsOnline(lista);
                    break;
            }
                      
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        private void PorcentajeLocalVsOnline(List<chart> lista)
        {
            var consultalocal = _context.Ventas.Include(x => x.DetalleVenta).Where(x => x.EsOnline == false).Count();
            var consultaonline = _context.Ventas.Include(x => x.DetalleVenta).Where(x => x.EsOnline == true).Count();                       
            lista.Add(new chart{ nombre = "Ventas Locales", valor = consultalocal });
            lista.Add(new chart{ nombre = "Ventas Online", valor = consultaonline });
        }

        private void VentasVsArriendos(List<chart> lista)
        {
            var consultaarriendo = _context.Arriendos.Include(x => x.DetalleArriendo).Count();
            var consultaVentas = _context.Ventas.Include(x => x.DetalleVenta).Count();            
            lista.Add(new chart{nombre = "Ventas",valor = consultaVentas});
            lista.Add(new chart{nombre = "Arriendos",valor = consultaarriendo});
        }

        private void VentasYarriendosTotales(List<chart> lista)
        {
            Random r = new Random();
            string[] colores  = { "#C0392B ", "#E74C3C", "#9B59B6", "#8E44AD", "#2980B9", "#3498DB", "#1ABC9C", "#16A085", "#27AE60", "#2ECC71", "#F1C40F", "#F39C12", "#E67E22", "#D35400" };
            var consultav = _context.DetalleVentas.Include(x => x.Producto).ToList();
            var consultaa = _context.DetalleArriendos.Include(x => x.Producto).ToList();
            
            var consultap = _context.Productos.Where(x=>x.TipoProductoId == Tipo_negocio.Seguridad).ToList();
            foreach (var item in consultap)
            {
               var a= consultav.Where(x => x.ProductoId == item.Id).Count();
               var b= consultaa.Where(x => x.ProductoId == item.Id).Count();
                lista.Add(new chart
                {
                    nombre = item.Nombre,
                    entero1 = a+b,
                    color = colores[r.Next(colores.Length)]
                });
            }
        }
                     
    }
}