using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Tienda.Models;
using Tienda.ViewModels;

namespace Tienda.Controllers
{
    [AllowAnonymous]
    public class InicioController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Index()
        {
            var productos = _context.Productos.Include(x => x.DetalleProducto).Where(x => x.TipoProductoId == Tipo_negocio.Seguridad).ToList();
            var lista = (List<AgregarProductoView>)Session["Cart"];

            if (lista != null)
                ViewBag.cartCount = lista.Count();

            return View(productos);
        }

        public ActionResult Detalles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var producto = _context.Productos.SingleOrDefault(x => x.Id == id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        public ActionResult MisCompras()
        {
            var ventas = _context.Ventas.Include(x=>x.Cliente).Where(x => x.Cliente.Email == User.Identity.Name).ToList();
           
            return View(ventas);
        }

        public ActionResult Detallecompras(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var detalleventas = _context.Ventas.Where(x=>x.Cliente.Email == User.Identity.Name).SingleOrDefault(x => x.Id == id);
            if (detalleventas == null)
            {
                return HttpNotFound();
            }
            return View(detalleventas);
        }

        public ActionResult Catalogo()
        {
            var lista = (List<AgregarProductoView>)Session["Cart"];
            ViewBag.cartCount = (lista == null) ? 0 : lista.Count();
            var productos = _context.Productos.Include(x => x.DetalleProducto).Where(x => x.TipoProductoId == Tipo_negocio.Seguridad).ToList();
            var categorias = _context.Categorias.Where(x => x.TipoProductoId == Tipo_negocio.Seguridad).ToList();
            return View(categorias);
        }
        [Authorize]
        public int AddCart(int ItemId)
        {
            var aPview = new AgregarProductoView();
            var _consultaProducto = _context.Productos.Where(p => p.Id == ItemId).SingleOrDefault();
            aPview.Cantidad = 1;
            if (Session["Cart"] == null)
            {
                List<AgregarProductoView> _cestaProductos = new List<AgregarProductoView>();
                AddCesta(aPview, _consultaProducto, _cestaProductos);
            }
            else
            {
                List<AgregarProductoView> _cestaProductos = (List<AgregarProductoView>)Session["Cart"];

                if (!(_cestaProductos.Where(l => l.IdProducto == ItemId).Count() >= 1))
                {
                    AddCesta(aPview, _consultaProducto, _cestaProductos);
                }
            }
            var lista = (List<AgregarProductoView>)Session["Cart"];
            ViewBag.cartCount = lista.Count();
            return lista.Count();
        }

        private void AddCesta(AgregarProductoView aPview, Producto _consultaProducto, List<AgregarProductoView> _cestaProductos)
        {
            aPview.IdProducto = _consultaProducto.Id;
            aPview.Nombre = _consultaProducto.Nombre;
            aPview.Precio = _consultaProducto.Precio;
            _cestaProductos.Add(aPview);
            Session["Cart"] = _cestaProductos;
        }

        public PartialViewResult GetCartItems()
        {
            var _view = new AgregarProductoView
            {
                DetalleCart = (List<AgregarProductoView>)Session["Cart"],
            };
            return PartialView("_ItemCesta", _view);
        }

        public PartialViewResult DeleteCart(int itemId)
        {
            var _cestaProductos = (List<AgregarProductoView>)Session["Cart"];
            if (itemId == 0)
            {
                _cestaProductos.Clear();
            }
            else
            {
                var _productoInCesta = _cestaProductos.Where(l => l.IdProducto == itemId).SingleOrDefault();
                _cestaProductos.RemoveAll(x => x.IdProducto == itemId);
            }
            return GetCartItems();
        }

        public PartialViewResult CargarLista(int? id)
        {
            var _view = _context.Productos.Include(x => x.DetalleProducto).Where(x => x.TipoProductoId == Tipo_negocio.Seguridad).ToList();

            if (id != null)
            {
                _view = _context.Productos.Include(x => x.DetalleProducto).Where(x => x.CategoriaId == id).Where(x => x.TipoProductoId == Tipo_negocio.Seguridad).ToList();
            }
            return PartialView("_ListaProductos", _view);
        }
        [Authorize]
        public ActionResult Crud()
        {
            var _listaproductos = (List<AgregarProductoView>)Session["Cart"];
            if (_listaproductos == null)
                return RedirectToAction("Catalogo", "Inicio");

            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GuardarCotizacion(CotizacionViewModel cotizacionView)
        {
            if (!ModelState.IsValid)
                return View("Crud", cotizacionView);


            var _consultaCliente = _context.Clientes.Where(x => x.Email == User.Identity.Name).FirstOrDefault();

            var _cestaProductos = (List<AgregarProductoView>)Session["Cart"];

            var _nuevaCotizacion = new Cotizacion()
            {
                ClienteId = _consultaCliente.Id,
                Fecha = DateTime.Now,
                EstadoId = Estados.Listo,
                EmpresaId = Empresas.Sostel,
                Comentario = cotizacionView.Comentario
            };
            _context.Cotizaciones.Add(_nuevaCotizacion);

            foreach (var item in _cestaProductos)
            {
                var _nuevoDetalle = new DetalleCotizacion()
                {
                    CotizacionId = _nuevaCotizacion.Id,
                    ProductoId = item.IdProducto,
                    Cantidad = item.Cantidad
                };

                _context.DetalleCotizaciones.Add(_nuevoDetalle);
            }
            Session["Cart"] = null;
            _context.SaveChanges();
            return RedirectToAction("Catalogo");
        }

        [Authorize]
        public ActionResult IniciarPago()
        {
            Random random = new Random();

            var DetalleCart = (List<AgregarProductoView>)Session["Cart"];
            var suma = 0;
            foreach (var item in DetalleCart)
            {
                suma += ((int)item.Precio * item.Cantidad);
            }

            var rmc = new ResponseModelcomercio
            {
                AuthorizedAmount = suma.ToString(),
                BuyOrder = random.Next(0, 1000).ToString(),
                SessionId = random.Next(0, 1000).ToString(),
                Urlreturn = "/inicio/mostrarvoucher",
                Urlfin = "/inicio/catalogo",
            };

            var tbknormal = new TransBank.Tbk_normal();
            var rm = new ResponseModel();
            rm = tbknormal.Tskmethod(rmc);

            if (rm.Response)
            {
                Session["token"] = rm.Token;

                return Redirect(string.Format("{0}?token_ws={1}", rm.Url, rm.Token));
            }
            else
            {
                var _view = new tsbproductoModel
                {
                    Mensaje = rm.Message,
                    Result = rm.Result
                };
                return View("_Formerror", _view);
            }
        }
        [Authorize]
        public void VentaOnline()
        {
            var _consultaCliente = _context.Clientes.Where(x => x.Email == User.Identity.Name).SingleOrDefault();
            var _consultaVendedor = _context.Users.Where(x => x.Rut == "0").SingleOrDefault();
            var _cestaProductos = (List<AgregarProductoView>)Session["Cart"];

            var _nuevaVenta = new Venta()
            {
                ClienteId = _consultaCliente.Id,
                DocumentoId = IdTipoDocumento.Factura,
                Fecha = DateTime.Now,
                EstadoId = Estados.PendianteDeEntrega,
                EmpresaId = Empresas.Sostel,
                EsOnline = true,
                TipoProductoId = Tipo_negocio.Seguridad,
                VendedorId = _consultaVendedor.Id
            };
            _context.Ventas.Add(_nuevaVenta);

            foreach (var item in _cestaProductos)
            {
                var _nuevoDetalle = new DetalleVenta()
                {
                    VentaId = _nuevaVenta.Id,
                    ProductoId = item.IdProducto,
                    Cantidad = item.Cantidad,
                    Precio = item.Precio
                };
                var _inventario = _context.Inventarios.Where(x => x.ProductoId == item.IdProducto).FirstOrDefault();
                _inventario.Stock -= item.Cantidad;

                _context.DetalleVentas.Add(_nuevoDetalle);
            }
           
            _context.SaveChanges();
            Session["Cart"] = null;


            Helpers.EmailHelper.EnviarEmail2(_nuevaVenta.Cliente.Email, "Mensaje de compra", comprobanteenviado(_nuevaVenta), "Su compra se a realizado correctamente, Pase a retirar sus productos a nuestro local ubicado en 24 1/2 Ote ; 20 1/2 Norte 3321, Talca. ", DatosCorreo.EmailVentas);
        }

        private StringReader comprobanteenviado(Venta _nuevaVenta)
        {
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    string companyName = "ASPSnippets";
                    int orderNo = 2303;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Order Sheet</b></td></tr>");
                    sb.Append("<tr><td colspan = '2'></td></tr>");
                    sb.Append("<tr><td><b>Order No:</b>");
                    sb.Append(orderNo);
                    sb.Append("</td><td><b>Date: </b>");
                    sb.Append(DateTime.Now);
                    sb.Append(" </td></tr>");
                    sb.Append("<tr><td colspan = '2'><b>Company Name :</b> ");
                    sb.Append(companyName);
                    sb.Append("</td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br />");
                    sb.Append("<table border = '1'>");
                    sb.Append("<tr>");

                    sb.Append("<th style = 'background-color: #D20B0C;color:#ffffff'>Id</th>");
                    sb.Append("<th style = 'background-color: #D20B0C;color:#ffffff'>Producto</th>");
                    sb.Append("<th style = 'background-color: #D20B0C;color:#ffffff'>Precio</th>");
                    sb.Append("<th style = 'background-color: #D20B0C;color:#ffffff'>Cantidad</th>");


                    sb.Append("</th>");
                    var a = _context.DetalleVentas.Include(x=>x.Producto).Where(x => x.VentaId == _nuevaVenta.Id).ToList();
                    sb.Append("</tr>");
                    foreach (var item in a)
                    {
                        sb.Append("<tr>");

                        sb.Append("<td>");
                        sb.Append(item.Id);
                        sb.Append("</td>");
                        sb.Append("<td>");
                        sb.Append(item.Producto.Nombre);
                        sb.Append("</td>");
                        sb.Append("<td>");
                        sb.Append(item.Precio);
                        sb.Append("</td>");
                        sb.Append("<td>");
                        sb.Append(item.Cantidad);
                        sb.Append("</td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                    StringReader sr = new StringReader(sb.ToString());

                  
                    return sr;
                }
            }
        }

        [Authorize]
        public PartialViewResult Cestasum(int id)
        {
            List<AgregarProductoView> _cestaProductos = (List<AgregarProductoView>)Session["Cart"];

            var _consultaProducto = _context.Productos.SingleOrDefault(x => x.Id == id);

            if (_cestaProductos.Where(l => l.IdProducto == id).Count() >= 1)
            {
                foreach (var item in _cestaProductos)
                {
                    if (item.IdProducto == id)
                    {
                        var cantidad = item.Cantidad;
                        if ((cantidad + 1) <= _consultaProducto.Inventario.Single().Stock && (cantidad + 1) > 0)
                        {
                            item.Cantidad += 1;
                        }
                    }
                }
            }
            return GetCartItems();
        }
        [Authorize]
        public PartialViewResult Cestarest(int id)
        {
            List<AgregarProductoView> _cestaProductos = (List<AgregarProductoView>)Session["Cart"];

            var _consultaProducto = _context.Productos.SingleOrDefault(x => x.Id == id);

            if (_cestaProductos.Where(l => l.IdProducto == id).Count() >= 1)
            {
                foreach (var item in _cestaProductos)
                {
                    if (item.IdProducto == id)
                    {
                        var cantidad = item.Cantidad;
                        if ((cantidad - 1) <= _consultaProducto.Inventario.Single().Stock && (cantidad - 1) > 0)
                        {
                            item.Cantidad--;
                        }
                    }
                }
            }
            return GetCartItems();
        }

        //RESULT
        public RedirectResult Mostrarvoucher()
        {
            var rmc = new ResponseModelcomercio { Tokentransaccion = Session["token"].ToString(), Action = "result" };
            var rm = new ResponseModel();
            var tbknormal = new TransBank.Tbk_normal();
            rm = tbknormal.Tskmethod(rmc);

            if (rm.Response)
            {
                VentaOnline();
                return Redirect(string.Format("{0}?token_ws={1}", rm.Url, rm.Token));
            }
            else
            {
                return Redirect("~/inicio/iniciarPago");
            }
        }

        public ActionResult Guardar(string nombre, string correo, string telefono, string mensaje)
        {
            var response = Request["g-recaptcha-response"];
            string secretKey = "6LdASDwUAAAAAI9pNFRnFk-yqZVpZfVXcWstds2c";
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify? secret ={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");

            if (status)
            {

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

            }
            else
            {
                ViewBag.Message = "Google reCaptcha validation failed";

            }
            return View("Index");
        }

    }
}