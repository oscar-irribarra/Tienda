using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
    public class Categoria
    {
        public Categoria()
        {
            Producto = new HashSet<Producto>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [MaxLength(100)]
        [Display(Name = "Categoria")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "En el campo {0} debe ingresar valores mayores a {1}")]
        [Display(Name = "Negocio")]
        public int TipoProductoId { get; set; }

        public virtual TipoProducto TipoProducto { get; set; }

        public virtual ICollection<Producto> Producto { get; set; }
    }
}

