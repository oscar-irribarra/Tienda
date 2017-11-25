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
    }
}