using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tienda.Models;

namespace Tienda.ViewModels
{
    public class AgregarProductoView
    {        
        [Range(1,int.MaxValue, ErrorMessage = "El campo {1} debe ser mayor a {1}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int Cantidad { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int Barcode { get; set; }

        

        public List<AgregarProductoView> DetalleCart { get; set; }

        public int IdProducto { get; set; }
        public int IdTipoProducto { get; set; }
        public string Nombre { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "El campo {1} debe ser mayor a {1}")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public double Precio { get; set; }
    }
}