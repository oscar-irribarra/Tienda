using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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

        public ActionResult IniciarPago()
        {
            var cesta = (List<AgregarProductoView>)Session["Cart"];

            if (cesta == null)
                return RedirectToAction("Catalogo");

            var _view = new tsbproductoModel
            {
                DetalleCart = cesta,
            };

            Session["CostoCesta"] = cesta.Sum(x => x.Precio * x.Cantidad);
            
            return View(_view);
        }
        
        public PartialViewResult FinalizarCompra()
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
                Urlreturn = "/inicio/Mostrarvoucher",
                Urlfin = "/inicio/catalogo"
            };

            var tbknormal = new TransBank.Tbk_normal();
            var rm = new ResponseModel();
            rm = tbknormal.tskmethod(rmc);
            if (rm.Response)
            {
                Session["token"] = rm.Token;

                var _view = new tsbproductoModel
                {
                    DetalleCart = (List<AgregarProductoView>)Session["Cart"],
                    tbviewModel = new TsbViewModel
                    {
                        action = rm.Url,
                        token = rm.Token
                    },
                    Mensaje = rm.Message
                };
                return PartialView("_Formtransbank", _view);
            }
            else
            {
                var _view = new tsbproductoModel
                {
                    Mensaje = rm.Message
                };
                return PartialView("_Formerror", _view);
            }
        }

        //RESULT
        public RedirectResult Mostrarvoucher()
        {
            var rmc = new ResponseModelcomercio { Tokentransaccion = Session["token"].ToString(), Action = "result" };
            var rm = new ResponseModel();
            var tbknormal = new TransBank.Tbk_normal();
            rm = tbknormal.tskmethod(rmc);

            if (rm.Response)
            {            
                return Redirect(string.Format("{0}?token_ws={1}", rm.Url, rm.Token));                                
            }
            else
            {            
                return Redirect("~/inicio/iniciarPago");
            }
        }
    }
}