﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tienda.Models;
using System.Data.Entity;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Tienda.Controllers
{
    [Authorize(Roles = Rol.Admin + "," + Rol.Vendedor)]
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

            return RedirectToAction("Index", "gestion");
        }

        public ActionResult Mensajes()
        {
            var mensaje = _context.Contactos.ToList();
            return View(mensaje);
        }

        public ActionResult Cotizaciones()
        {
            var _cotizaciones = _context.Cotizaciones.ToList();
            return View(_cotizaciones);
        }

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

        public JsonResult Get(int a, int b)
        {
            List<chart> lista = new List<chart>();
            switch (a)
            {
                case 1:
                    VentasYarriendosTotales(lista, b);
                    break;
                case 2:
                    PorcentajeLocalVsOnline(lista, b);
                    break;
                case 3:
                    VentasVsArriendos(lista, b);
                    break;
                case 4:
                    for (int i = 1; i <= 12; i++)
                    {
                        var suma = 0.0;
                        try
                        {
                            suma = _context.DetalleVentas.Where(x => x.Venta.Fecha.Month == i).Sum(x => x.Precio);
                        }
                        catch (Exception)
                        {

                            suma = 0;
                        }
                        var asd = i;

                        string[] meses = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Nobiembre", "Diciembre" };

                        lista.Add(new chart { nombre = meses[asd - 1], valor = suma });
                    }
                    break;
                case 5:
                    for (int i = 1; i <= 12; i++)
                    {
                        var suma = 0.0;
                        try
                        {
                            suma = _context.DetalleAdquisiciones.Where(x => x.Adquisicion.Fecha.Month == i).Sum(x => x.Precio);
                        }
                        catch (Exception)
                        {

                            suma = 0;
                        }
                        var asd = i;

                        string[] meses = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Nobiembre", "Diciembre" };

                        lista.Add(new chart { nombre = meses[asd - 1], valor = suma });
                    }
                    break;
                default:
                    PorcentajeLocalVsOnline(lista, b);
                    break;
            }

            return Json(lista, JsonRequestBehavior.AllowGet);
        }
        private void cantidadventa_año(List<chart> lista)
        {


        }

        private void PorcentajeLocalVsOnline(List<chart> lista, int b)
        {
            var consultalocal = 0;
            var consultaonline = 0;
            if (b == 0)
            {
                consultalocal = _context.Ventas.Include(x => x.DetalleVenta).Where(x => x.EsOnline == false).Count();
                consultaonline = _context.Ventas.Include(x => x.DetalleVenta).Where(x => x.EsOnline == true).Count();
            }
            else
            {
                consultalocal = _context.Ventas.Include(x => x.DetalleVenta).Where(x => x.EsOnline == false).Where(x => x.Fecha.Month == b).Count();
                consultaonline = _context.Ventas.Include(x => x.DetalleVenta).Where(x => x.EsOnline == true).Where(x => x.Fecha.Month == b).Count();
            }

            lista.Add(new chart { nombre = "Ventas Locales", valor = consultalocal });
            lista.Add(new chart { nombre = "Ventas Online", valor = consultaonline });
        }

        private void VentasVsArriendos(List<chart> lista, int b)
        {
            var consultaarriendo = 0;
            var consultaVentas = 0;
            if (b == 0)
            {
                consultaarriendo = _context.Arriendos.Include(x => x.DetalleArriendo).Count();
                consultaVentas = _context.Ventas.Include(x => x.DetalleVenta).Count();
            }
            else
            {
                consultaarriendo = _context.Arriendos.Include(x => x.DetalleArriendo).Where(x => x.FechaInicio.Month == b).Count();
                consultaVentas = _context.Ventas.Include(x => x.DetalleVenta).Where(x => x.Fecha.Month == b).Count();
            }

            lista.Add(new chart { nombre = "Ventas", valor = consultaVentas });
            lista.Add(new chart { nombre = "Arriendos", valor = consultaarriendo });
        }

        private void VentasYarriendosTotales(List<chart> lista, int b)
        {
            Random r = new Random();
            string[] colores = { "#C0392B ", "#E74C3C", "#9B59B6", "#8E44AD", "#2980B9", "#3498DB", "#1ABC9C", "#16A085", "#27AE60", "#2ECC71", "#F1C40F", "#F39C12", "#E67E22", "#D35400" };
            var consultav = _context.DetalleVentas.Include(x => x.Producto).ToList();
            var consultaa = _context.DetalleArriendos.Include(x => x.Producto).ToList();

            if (b != 0)
            {
                consultav = _context.DetalleVentas.Include(x => x.Producto).Where(x => x.Venta.Fecha.Month == b).ToList();
                consultaa = _context.DetalleArriendos.Include(x => x.Producto).Where(x => x.Arriendo.FechaInicio.Month == b).ToList();
            }



            var consultap = _context.Productos.Where(x => x.TipoProductoId == Tipo_negocio.Seguridad).ToList();
            foreach (var item in consultap)
            {
                var a = consultav.Where(x => x.ProductoId == item.Id).Sum(x => x.Cantidad);
                var c = consultaa.Where(x => x.ProductoId == item.Id).Sum(x => x.Cantidad);
                lista.Add(new chart
                {
                    nombre = item.Nombre,
                    entero1 = a + c,
                    color = colores[r.Next(colores.Length)]
                });
            }
        }


    }
}