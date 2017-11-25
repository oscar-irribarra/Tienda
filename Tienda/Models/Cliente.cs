using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda.Models
{
    public class Cliente
    {
        public Cliente()
        {
            Venta = new HashSet<Venta>();
            Arriendo = new HashSet<Arriendo>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [MaxLength(12, ErrorMessage = "El campo{0} puede tener maximo {1} Caracteres")]
        [Index("Cliente_Rut_Index", IsUnique = true)]
        public string Rut { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [MaxLength(100)]
        public string Apellido { get; set; }
        
        [MaxLength(100)]
        public string Comuna { get; set; }
        
        [MaxLength(100)]
        public string Ciudad { get; set; }
        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        public int Telefono { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }

        [Display(Name = "Nombre Completo")]
        public string FullName { get { return string.Format("{0} {1}", Nombre, Apellido); } }

        public virtual ICollection<Venta> Venta { get; set; }     
        public virtual ICollection<Arriendo> Arriendo { get; set; }  
    }
}