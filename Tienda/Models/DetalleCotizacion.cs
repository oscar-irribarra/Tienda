using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tienda.Models
{
    [Table("DetalleCotizacion")]
    public class DetalleCotizacion
    {
        public int Id { get; set; }

        [Required]
        public int CotizacionId { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [Required]
        public int Cantidad { get; set; }
         
        public virtual Producto Producto { get; set; }
        public virtual Cotizacion Cotizacion { get; set; }
    }
}