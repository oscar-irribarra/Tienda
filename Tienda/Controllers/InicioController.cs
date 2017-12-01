using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tienda.Models;
using System.Data.Entity;
using System.Net;
using Tienda.ViewModels;
using Webpay.Transbank.Library;
using Webpay.Transbank.Library.Wsdl.Normal;
using Webpay.Transbank.Library.Wsdl.Nullify;

namespace Tienda.Controllers
{
    [AllowAnonymous]
    public class InicioController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Index()
        {          
            var productos = _context.Productos.Include(x => x.DetalleProducto).Where(x => x.TipoProductoId == Tipo_negocio.Seguridad).ToList();
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
            var productos = _context.Productos.Include(x => x.DetalleProducto).Where(x => x.TipoProductoId == Tipo_negocio.Seguridad).ToList();
            return View(productos);
        }

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
            var _productoInCesta = _cestaProductos.Where(l => l.IdProducto == itemId).SingleOrDefault();
            _cestaProductos.RemoveAll(x => x.IdProducto == itemId);
            return GetCartItems();
        }


        private string message;

        /** Crea Dictionary con datos Integración Pruebas */
        private Dictionary<string, string> certificate = Tienda.TransBank.cert_normal.certificate();

        /** Crea Dictionary con datos de entrada */
        private Dictionary<string, string> request = new Dictionary<string, string>();
        
        public ActionResult IniciarPago()
        {
            var cesta = (List<AgregarProductoView>)Session["Cart"];
            var _view = new tsbproductoModel
            {
                DetalleCart = cesta,
            };
            Session["CostoCesta"] = cesta.Sum(x => x.Precio * x.Cantidad);
            return View(_view);
        }

        public PartialViewResult Mostrarvoucher()
        {
            Webpay.Transbank.Library.Webpay webpay;
            string sample_baseurl;

            tsbConfig(out webpay, out sample_baseurl);

            Dictionary<string, string> codes = new Dictionary<string, string>();

            codes.Add("0", "Transacci&oacute;n aprobada");
            codes.Add("-1", "Rechazo de transacci&oacute;n");
            codes.Add("-2", "Transacci&oacute;n debe reintentarse");
            codes.Add("-3", "Error en transacci&oacute;n");
            codes.Add("-4", "Rechazo de transacci&oacute;n");
            codes.Add("-5", "Rechazo por error de tasa");
            codes.Add("-6", "Excede cupo m&aacute;ximo mensual");
            codes.Add("-7", "Excede l&iacute;mite diario por transacci&oacute;n");
            codes.Add("-8", "Rubro no autorizado");
            try
            {
                /** Token de la transacción */
                string token = Session["token"].ToString();

                request.Add("token", token.ToString());

                transactionResultOutput result = webpay.getNormalTransaction().getTransactionResult(token);
                          
                if (result.detailOutput[0].responseCode == 0)
                {
                    message = "Pago ACEPTADO por webpay (se deben guardar datos para mostrar voucher)";                                  
                }
                else
                {
                    message = "Pago RECHAZADO por webpay [Codigo]=> " + result.detailOutput[0].responseCode + " [Descripcion]=> " + codes[result.detailOutput[0].responseCode.ToString()];
                }
                var _view = new tsbproductoModel
                {
                    DetalleCart = (List<AgregarProductoView>)Session["Cart"],
                    tbviewModel = new TsbViewModel
                    {
                        action = result.urlRedirection,
                        token = token,
                    },
                    Mensaje = message,
                    Tipo = "result"
                };

                return PartialView("_Formtransbank", _view);
            }
            catch (Exception ex)
            {
                var _view = new tsbproductoModel
                {
                    Mensaje = "Hubo un error " + ex.Message
                };
                return PartialView("_Formerror", _view);
            }
            
        }
        public PartialViewResult FinalizarCompra()
        {
            Webpay.Transbank.Library.Webpay webpay;
            string sample_baseurl;

            tsbConfig(out webpay, out sample_baseurl);

            try
            {
                var DetalleCart = (List<AgregarProductoView>)Session["Cart"];
                var suma = 0;
                foreach (var item in DetalleCart)
                {
                    suma += ((int)item.Precio * item.Cantidad);
                }
                Random random = new Random();

                /** Monto de la transacción */
                decimal amount = System.Convert.ToDecimal(suma);

                /** Orden de compra de la tienda */
                string buyOrder = random.Next(0, 1000).ToString();

                /** (Opcional) Identificador de sesión, uso interno de comercio */
                string sessionId = random.Next(0, 1000).ToString();

                /** URL Final */
                string urlReturn = sample_baseurl + "/inicio/Mostrarvoucher";

                /** URL Final */
                string urlFinal = sample_baseurl + "/inicio/catalogo";

                request.Add("amount", amount.ToString());
                request.Add("buyOrder", buyOrder.ToString());
                request.Add("sessionId", sessionId.ToString());
                request.Add("urlReturn", urlReturn.ToString());
                request.Add("urlFinal", urlFinal.ToString());

                /** Ejecutamos metodo initTransaction desde Libreria */
                wsInitTransactionOutput result = webpay.getNormalTransaction().initTransaction(amount, buyOrder, sessionId, urlReturn, urlFinal);

                /** Verificamos respuesta de inicio en webpay */
                if (result.token != null && result.token != "")
                {
                    message = "Sesion iniciada con exito en Webpay";
                    Session["token"] = result.token;
                }
                else
                {
                    message = "webpay no disponible";
                }

                var _view = new tsbproductoModel
                {
                    DetalleCart = (List<AgregarProductoView>)Session["Cart"],
                    tbviewModel = new TsbViewModel
                    {
                        action = result.url,
                        token = result.token,
                        codigoautorizacion = buyOrder
                    },
                    Mensaje = message,
                    Tipo = "init"
                };

                return PartialView("_Formtransbank", _view);
                
            }
            catch (Exception ex)
            {
                var _view = new tsbproductoModel
                {
                    Mensaje = "Hubo un error " + ex.Message
                };
                return PartialView("_Formerror", _view);
            }
        }

        private void tsbConfig(out Webpay.Transbank.Library.Webpay webpay, out string sample_baseurl)
        {
            Configuration configuration = new Configuration();
            configuration.Environment = certificate["environment"];
            configuration.CommerceCode = certificate["commerce_code"];
            configuration.PublicCert = certificate["public_cert"];
            configuration.WebpayCert = certificate["webpay_cert"];
            configuration.Password = certificate["password"];

            /** Creacion Objeto Webpay */
            webpay = new Webpay.Transbank.Library.Webpay(configuration);

            /** Información de Host para crear URL */
            String httpHost = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString();
            String selfURL = System.Web.HttpContext.Current.Request.ServerVariables["URL"].ToString();

            /** Crea URL de Aplicación */
            sample_baseurl = "http://" + httpHost;

            /** Crea Dictionary con codigos de resultado */
           
        }

    }
}