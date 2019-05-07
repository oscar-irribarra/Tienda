using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
    public class Venta
    {
        public Venta()
        {
            DetalleVenta = new HashSet<DetalleVenta>();
        }
        public int Id { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Required]
        public int DocumentoId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        [Required]
        public int EstadoId { get; set; }

        [Required]
        public int TipoProductoId { get; set; }

        [Required]
        public bool EsOnline { get; set; }
        
        [Required]
        public string VendedorId { get; set; }
        public virtual ApplicationUser Vendedor { get; set; }

        public virtual Cliente Cliente { get; set; } 
        public virtual TipoProducto TipoProducto { get; set; }
        public virtual Empresa Empresa { get; set; }        
        public virtual Estado Estado { get; set; }                    
        public virtual Documento Documento { get; set; }
        public virtual ICollection<DetalleVenta> DetalleVenta { get; set; }
    }
}

