using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tienda.ViewModels
{
    public class TsbViewModel
    {
        public string action { get; set; }
        public string token { get; set; }
        public string codigoautorizacion { get; set; }
    }
    public class tsbproductoModel
    {
        public List<AgregarProductoView> DetalleCart { get; set; }
        public TsbViewModel tbviewModel { get; set; }
        public string Mensaje { get; set; }
        public string Tipo { get; set; }
        public string Result { get; set; }
    }
}