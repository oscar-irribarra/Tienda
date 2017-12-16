using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
                _view = _context.Productos.Where(x => x.CategoriaId == id).Where(x => x.TipoProductoId == Tipo_negocio.Seguridad).ToList();
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
            Session["Cart"] = null;
            _context.SaveChanges();
            Helpers.EmailHelper.EnviarEmail2(_nuevaVenta.Cliente.Email, "Mensaje de compra", "Used realizo una compra", DatosCorreo.EmailVentas);
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