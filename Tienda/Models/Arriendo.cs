using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda.Models
{
    public class Arriendo
    {
        public Arriendo()
        {
            DetalleArriendo = new HashSet<DetalleArriendo>();
        }
        public int Id { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        [Required]
        public int DocumentoId { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int EstadoId { get; set; }

        [Required]
        public int EmpresaId { get; set; }
        [Required]
        public int TipoProductoId { get; set; }

        [Required]
        public string VendedorId { get; set; }
        public virtual ApplicationUser Vendedor { get; set; }

        public virtual TipoProducto TipoProducto { get; set; }
        public virtual Empresa Empresa { get; set; }       
        public virtual Cliente Cliente { get; set; }       
        public virtual Estado Estado { get; set; }          
        public virtual Documento Documento { get; set; }
        
        public virtual ICollection<DetalleArriendo> DetalleArriendo { get; set; }
    }
}
