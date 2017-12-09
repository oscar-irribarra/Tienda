using System;
using System.IO;
using System.Web;
using Tienda.Models;

namespace Tienda.Helpers
{
    public class ImagenHelper
    {
        public static bool UploadPhoto(HttpPostedFileBase file, string folder, string nombre)
        {
            if (file == null || string.IsNullOrEmpty(folder) || string.IsNullOrEmpty(nombre))
                return false;

            try
            {
                string path = string.Empty;
                string pic = string.Empty;

                if (file != null)
                {
                    path = Path.Combine(HttpContext.Current.Server.MapPath(folder), nombre);
                    file.SaveAs(path);                   
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);
                        byte[] array = ms.GetBuffer();
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool SubirImagen(DetalleProducto detalleproducto)
        {
            if (detalleproducto.ImagenFile != null)
            {
                var folder = "~/Content/ImagenesProductos";
                var file = string.Format("{0}_{1}.jpg", detalleproducto.Id, detalleproducto.ProductoId);
                var respuesta = UploadPhoto(detalleproducto.ImagenFile, folder, file);

                if (respuesta)
                {
                    var pic = string.Format("{0}/{1}", folder, file);
                    detalleproducto.ImagenUrl = pic;
                    return true;
                }
            }
            return false;
        }
    }
}