using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Hosting;
using Tienda.Models;
using Webpay.Transbank.Library;
using Webpay.Transbank.Library.Wsdl.Normal;
using Webpay.Transbank.Library.Wsdl.Nullify;
namespace Tienda.TransBank
{
    public class Tbk_normal
    {       
        /** Crea Dictionary con datos de entrada */
        private Dictionary<string, string> request = new Dictionary<string, string>();
        public ResponseModel Tskmethod(ResponseModelcomercio rmc)
        {
            var logFileNameWithPath = HttpContext.Current.Server.MapPath(@"~\Content\597020000541\");
            //String certFolder = HttpContext.Current.Server.MapPath("~//Content/597020000541/");
            var certFolder = HostingEnvironment.MapPath("~//");

            Configuration configuration = new Configuration
            {
                Environment = "INTEGRACION",
                CommerceCode = "597020000541",
                PublicCert = logFileNameWithPath+ "tbk.pem",
                WebpayCert = logFileNameWithPath + "597020000541.pfx",
                Password = "transbank123",
            };

            Webpay.Transbank.Library.Webpay webpay = new Webpay.Transbank.Library.Webpay(configuration);        

            /** Información de Host para crear URL */
           

            ///** Crea Dictionary con descripción */
            Dictionary<string, string> description = new Dictionary<string, string>();

            description.Add("VD", "Venta Debito");
            description.Add("VN", "Venta Normal");
            description.Add("VC", "Venta en cuotas");
            description.Add("SI", "3 cuotas sin interés");
            description.Add("S2", "2 cuotas sin interés");
            description.Add("NC", "N Cuotas sin interés");

            ///** Crea Dictionary con codigos de resultado */
            Dictionary<string, string> codes = new Dictionary<string, string>
            {
                { "0", "Transacción aprobada" },
                { "-1", "Rechazo de transacción" },
                { "-2", "Transacción debe reintentarse" },
                { "-3", "Error en transacción" },
                { "-4", "Rechazo de transacción" },
                { "-5", "Rechazo por error de tasa" },
                { "-6", "Excede cupo máximo mensual." },
                { "-7", "Excede límite diario por transacción" },
                { "-8", "Rubro no autorizado" }
            };

            String httpHost = HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString();
            String selfURL = HttpContext.Current.Request.ServerVariables["URL"].ToString();

            /** Crea URL de Aplicación */
            string sample_baseurl = "http://" + httpHost;
            var rm = new ResponseModel();

            string buyOrder;

            switch (rmc.Action)
            {
                default:
                    try
                    {
                        /** Monto de la transacción */
                        decimal amount = Convert.ToDecimal(rmc.AuthorizedAmount);

                        /** Orden de compra de la tienda */
                        buyOrder = rmc.BuyOrder;

                        /** (Opcional) Identificador de sesión, uso interno de comercio */
                        string sessionId = rmc.SessionId;

                        /** URL Final */
                        string urlReturn = sample_baseurl + rmc.Urlreturn;

                        /** URL Final */
                        string urlFinal = sample_baseurl + rmc.Urlfin;

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
                            rm.Message = "Sesion iniciada con exito en Webpay";
                        }
                        else
                        {
                            rm.Message = "webpay no disponible";
                        }

                        rm.Request = "REQUEST: " + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request); 
                        rm.Result = "RESULT: " + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result);
                        rm.Url = result.url;
                        rm.Token = result.token;
                        rm.Response = true;
                    }
                    catch (Exception ex)
                    {
                        rm.Request = "REQUEST: " + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request);
                        rm.Result = "RESULT: Ocurrío; un error en la transacción (Validar correcta configuracíon de parametros). " + ex.Message + "";
                        
                        rm.Message = certFolder + "Content/597020000541/tbk.pem";
                        rm.Response = false;
                     
                    }
                    break;

                case "result":

                    try
                    {
                        /** Token de la transacción */
                        string token = rmc.Tokentransaccion;

                        request.Add("token", token.ToString());

                        transactionResultOutput result = webpay.getNormalTransaction().getTransactionResult(token);
                        rm.Request = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request);
                        rm.Result = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result);

                        if (result.detailOutput[0].responseCode == 0)
                        {
                            rm.Message = "Pago ACEPTADO por webpay (se deben guardar datos para mostrar voucher)";

                            HttpContext.Current.Response.Write("<script>localStorage.setItem('authorizationCode', " + result.detailOutput[0].authorizationCode + ")</script>");
                            HttpContext.Current.Response.Write("<script>localStorage.setItem('commercecode', " + result.detailOutput[0].commerceCode + ")</script>");
                            HttpContext.Current.Response.Write("<script>localStorage.setItem('amount', " + result.detailOutput[0].amount + ")</script>");
                            HttpContext.Current.Response.Write("<script>localStorage.setItem('buyOrder', " + result.detailOutput[0].buyOrder + ")</script>");
                        }
                        else
                        {
                            rm.Message = "Pago RECHAZADO por webpay [Codigo]=> " + result.detailOutput[0].responseCode + " [Descripcion]=> " + codes[result.detailOutput[0].responseCode.ToString()];
                        }

                        rm.Url = result.urlRedirection;
                        rm.Token = token;
                        rm.Response = true;
                    }
                    catch (Exception ex)
                    {
                        rm.Request = "REQUEST: " + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request);
                        rm.Result = "RESULT: Ocurrío; un error en la transacción (Validar correcta configuracíon de parametros). " + ex.Message + "";
                        rm.Response = false;
                    }

                    break;

                case "end":
                    
                    try
                    {
                        request.Add("", "");

                        rm.Request = "REQUEST: " + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "";
                        rm.Result = "RESULT: " + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(rmc.Tokentransaccion) + "";

                        rm.Message = "Transacci&oacute;n Finalizada";

                        string next_page = sample_baseurl+rmc.Urlend;

                        HttpContext.Current.Response.Write("<form action=" + next_page + " method='post'><input type='hidden' name='commercecode' id='commercecode' value=''><input type='hidden' name='authorizationCode' id='authorizationCode' value=''><input type='hidden' name='amount' id='amount' value=''><input type='hidden' name='buyOrder' id='buyOrder' value=''><input type='submit' value='Anular Transacci&oacute;n &raquo;'></form>");
                        HttpContext.Current.Response.Write("<script>var commercecode = localStorage.getItem('commercecode');document.getElementById('commercecode').value = commercecode;</script>");
                        HttpContext.Current.Response.Write("<script>var authorizationCode = localStorage.getItem('authorizationCode');document.getElementById('authorizationCode').value = authorizationCode;</script>");
                        HttpContext.Current.Response.Write("<script>var amount = localStorage.getItem('amount');document.getElementById('amount').value = amount;</script>");
                        HttpContext.Current.Response.Write("<script>var buyOrder = localStorage.getItem('buyOrder');document.getElementById('buyOrder').value = buyOrder;</script>");
                        rm.Response = true;

                    }
                    catch (Exception ex)
                    {
                        rm.Request = "REQUEST: " + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request);
                        rm.Result = "RESULT: Ocurrío; un error en la transacción (Validar correcta configuracíon de parametros). " + ex.Message + "";
                        rm.Response = false;
                    }

                    break;

                case "nullify":
                    
                    try
                    {
                        /** Codigo de Comercio */
                        string commercecode = rmc.Commercecode;

                        /** Código de autorización de la transacción que se requiere anular */
                        string authorizationCode = rmc.AuthorizationCode;

                        /** Monto autorizado de la transacción que se requiere anular */
                        decimal authorizedAmount = Int64.Parse(rmc.AuthorizedAmount);

                        /** Orden de compra de la transacción que se requiere anular */
                        buyOrder = rmc.BuyOrder;

                        /** Monto que se desea anular de la transacción */
                        decimal nullifyAmount = Convert.ToDecimal(rmc.NullifyAmount);

                        request.Add("authorizationCode", authorizationCode.ToString());
                        request.Add("authorizedAmount", authorizedAmount.ToString());
                        request.Add("buyOrder", buyOrder.ToString());
                        request.Add("nullifyAmount", nullifyAmount.ToString());
                        request.Add("commercecode", commercecode.ToString());

                        nullificationOutput resultNullify = webpay.getNullifyTransaction().nullify(authorizationCode, authorizedAmount, buyOrder, nullifyAmount, commercecode);

                        rm.Request = "REQUEST: " + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "";
                        rm.Result = "RESULT: " + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(resultNullify) + "";

                        rm.Message = "Transacci&oacute;n Finalizada";
                        rm.Response = true;
                    }
                    catch (Exception ex)
                    {
                        rm.Request = "REQUEST: " + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(request) + "";
                        rm.Result = "RESULT: Ocurrio; un error en la transaccion (Validar correcta configuracíon de parametros). " + ex.Message + "";
                        rm.Response = false;
                    }

                    break;

            }
            return rm;
        }


    }


}