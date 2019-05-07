using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Tienda.Models
{
    [Table("Cotizacion")]
    public class Cotizacion
    {
        public Cotizacion()
        {
            DetalleCotizacion = new HashSet<DetalleCotizacion>();
        }
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comentario { get; set; }

        [Required]
        public int ClienteId { get; set; }

        [Required]
        public int EstadoId { get; set; }

        [Required]
        public int EmpresaId { get; set; }
                    
        public virtual Empresa Empresa { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Estado Estado { get; set; }

        public virtual ICollection<DetalleCotizacion> DetalleCotizacion { get; set; }
    }
}