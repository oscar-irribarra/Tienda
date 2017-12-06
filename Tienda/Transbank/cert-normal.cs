using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tienda.TransBank
{
    public class cert_normal
    {
        internal static Dictionary<string, string> certificate()
        {

            /** Crea un Dictionary para almacenar los datos de integración pruebas */
            Dictionary<string, string> certificate = new Dictionary<string, string>();

            String certFolder = HttpContext.Current.Server.MapPath("~/Content/597020000541/");
            //C:\Users\Osc-r\Documents\Visual Studio 2017\Projects\Tienda\Tienda\Content\597020000541\597020000541.pfx
            /** Modo de Utilización */
            certificate.Add("environment", "INTEGRACION");

            /** Certificado Publico (Dirección fisica de certificado o contenido) */
            certificate.Add("public_cert", certFolder + "tbk.pem");

            /** Ejemplo de Ruta de Certificado de Salida */
            certificate.Add("webpay_cert", certFolder + "597020000541.pfx");

            /** Ejemplo de Password de Certificado de Salida */
            certificate.Add("password", "transbank123");

            /** Codigo Comercio */
            certificate.Add("commerce_code", "597020000541");

            return certificate;

        }
    }
}