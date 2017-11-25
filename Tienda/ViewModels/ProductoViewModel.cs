using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tienda.Models;

namespace Tienda.ViewModels
{
    public class ProductoViewModel
    {
        public Producto Producto { get; set; }
        public DetalleProducto DetallesProducto { get; set; }
        public Inventario Inventario { get; set; }
    }
}