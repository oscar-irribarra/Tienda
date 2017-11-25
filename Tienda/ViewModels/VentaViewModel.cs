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

        public bool Ispublica { get; set; }
    }

    public class AdquisionViewModel
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int DocumentoID { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Rut { get; set; }
    }

    public class ArriendoViewModel
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int DocumentoID { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Rut { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }
    }
}