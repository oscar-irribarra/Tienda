using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda.Models
{
    public class Contacto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
        [Required]
        public string Nombre { get; set; }
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [DataType(DataType.EmailAddress)]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [DataType(DataType.MultilineText)]
        [MaxLength(350)]
        public string Contenido { get; set; }
        public string Ip { get; set; }
        [Required]
        public int EstadoId { get; set; }

        
        public virtual Estado Estado { get; set; }  
    }
}

