using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tienda.ViewModels
{
    public class VentaViewModel
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int DocumentoID { get; set; }

    
   
        public string Rut { get; set; }        
        
     
        [MaxLength(100)]
        public string Nombre { get; set; }

       
        [MaxLength(100)]
        public string Apellido { get; set; }

     
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool Ispublica { get; set; }
    }

    public class CotizacionViewModel
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.MultilineText)]
        public string Comentario { get; set; }
    }

    public class AdquisionViewModel
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int DocumentoID { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Rut { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Ingrese un {0} Valido")]
        public string Correo { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }
    }

    public class ArriendoViewModel
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int DocumentoID { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Rut { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [MaxLength(100)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }


        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }


        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [DisplayFormat(DataFormatString = "{0:c0}")]
        public double Precio { get; set; }
    }
}