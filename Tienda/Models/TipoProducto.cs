using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tienda.Models
{
    public class TipoProducto
    {
        public TipoProducto()
        {
            Producto = new HashSet<Producto>();
            Categoria = new HashSet<Categoria>();
            Venta = new HashSet<Venta>();
            Adquisicion = new HashSet<Adquisicion>();
            Arriendo = new HashSet<Arriendo>();

        }
        public int Id { get; set; }     
        public string Nombre { get; set; }      

        [Required]
        public int EmpresaId { get; set; }

        public virtual Empresa Empresa { get; set; }

        public virtual ICollection<Producto> Producto { get; set; }
        public virtual ICollection<Categoria> Categoria { get; set; }
        public virtual ICollection<Venta> Venta { get; set; }
        public virtual ICollection<Adquisicion> Adquisicion { get; set; }
        public virtual ICollection<Arriendo> Arriendo { get; set; }     
    }

    static class Tipo_negocio
    {        
        public const int Minimarket = 1;
        public const int Seguridad = 2;
    }
}