using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tienda.Models;

namespace Tienda.ViewModels
{
    public class FormProductoViewModel
    {     
        public int Cantidad { get; set; }        

        public DetalleProducto DetalleProductofrm { get; set; }

        public Producto Productofrm { get; set; }

    }
}