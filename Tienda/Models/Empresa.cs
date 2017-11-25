using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tienda.Models
{
    public class Empresa
    {
        public Empresa()
        {
            TipoProducto = new HashSet<TipoProducto>();
            Venta = new HashSet<Venta>();
            Arriendo = new HashSet<Arriendo>();
            Adquisicion = new HashSet<Adquisicion>();
        }
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Rut { get; set; }

        [Required]
        [MaxLength(100)]
        public string RazonSocial { get; set; }

        [Required]
        [MaxLength(100)]
        public string Giro { get; set; }

        [Required]
        [MaxLength(100)]
        public string Direccion { get; set; }

        [Required]
        [MaxLength(100)]
        public string Comuna { get; set; }

        [Required]
        [MaxLength(100)]
        public string Ciudad { get; set; }

        [Required]
        [MaxLength(150)]
        public string RepresentanteLegal { get; set; }

        public int Telefono { get; set; }

        public string Email { get; set; }


        public virtual ICollection<TipoProducto> TipoProducto { get; set; }

        public virtual ICollection<Venta> Venta { get; set; }

        public virtual ICollection<Arriendo> Arriendo { get; set; }

        public virtual ICollection<Adquisicion> Adquisicion { get; set; }
    }

    static class Empresas
    {
       public const int Sostel = 1;
    }
}