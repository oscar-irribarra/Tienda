using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda.Models
{
    public class Documento
    {
        public Documento()
        {
            Venta = new HashSet<Venta>();
            Adquisicion = new HashSet<Adquisicion>();
            Arriendo = new HashSet<Arriendo>();
        }
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        public int Codigo { get; set; }



        public virtual ICollection<Venta> Venta { get; set; }
        public virtual ICollection<Adquisicion> Adquisicion { get; set; }
        public virtual ICollection<Arriendo> Arriendo { get; set; }
    }
    static class IdTipoDocumento
    {
        public const int Factura = 1;
    }
    static class TipoDocumento
    {
        public const string Factura = "Factura";
        public const string Boleta = "Boleta";
    }
}