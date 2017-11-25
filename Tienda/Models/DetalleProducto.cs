using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Tienda.Models
{    
    public class DetalleProducto
    {       
        public int Id { get; set; }

        public string Marca { get; set; }

        public string Modelo { get; set; }

        public string Color { get; set; }

        public string Descripcion { get; set; }

        public string ImagenUrl { get; set; }
       
        public int ProductoId { get; set; }

        [NotMapped]
        [Display(Name = "Imagen")]
        public HttpPostedFileBase ImagenFile { get; set; }

        
        public virtual Producto Producto { get; set; }

    }
}