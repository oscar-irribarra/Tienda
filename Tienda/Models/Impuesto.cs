using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda.Models
{
    public class Impuesto
    {
        public Impuesto()
        {
            Producto = new HashSet<Producto>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(50)]
        [Display(Name = "Impuesto")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public double Tasa { get; set; }

        public virtual ICollection<Producto> Producto { get; set; }
    }
}

