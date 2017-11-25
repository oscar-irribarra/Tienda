using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda.Models
{   
    public class DetalleVenta
    {       
        public int Id { get; set; }

        [Required]
        public int VentaId { get; set; }

        [Required]        
        public int ProductoId  { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:c0}")]
        public double Precio { get; set; }


        public virtual Producto Producto { get; set; }        
        public virtual Venta Venta { get; set; }
    }
}

