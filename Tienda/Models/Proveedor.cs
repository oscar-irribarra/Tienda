using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda.Models
{
    public class Proveedor
    {
        public Proveedor()
        {
            Adquisicion = new HashSet<Adquisicion>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(12, ErrorMessage = "El campo{0} puede tener maximo {1} Caracteres")]       
        [Index("Proveedor_Rut_Index", IsUnique = true)]
        public string Rut { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(100)]       
        public string Nombre  { get; set; }

        [MaxLength(100)] 
        [EmailAddress(ErrorMessage = "Ingrese un {0} Valido")]
        public string Correo { get; set; }
        
        public int? Telefono { get; set; }   
        
        public virtual ICollection<Adquisicion> Adquisicion { get; set; }    
    }
}