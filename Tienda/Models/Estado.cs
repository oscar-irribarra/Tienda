using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda.Models
{
    public class Estado
    {
        public Estado()
        {
            Adquisicion = new HashSet<Adquisicion>();
            Venta = new HashSet<Venta>();
            Arriendo = new HashSet<Arriendo>();
            Contacto = new HashSet<Contacto>();
        }
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        public virtual ICollection<Adquisicion> Adquisicion { get; set; }
        public virtual ICollection<Venta> Venta { get; set; }
        public virtual ICollection<Arriendo> Arriendo { get; set; }
        public virtual ICollection<Contacto> Contacto { get; set; }

        //Factura
        //public virtual ICollection<Caf> Caf { get; set; }    
    }

    static class Estados
    {        
        public const int Listo = 1;
        public const int Cancelado = 2;
        public const int Finalizada = 3;
        public const int EnCurso = 4;
    }
    static class TipoEstado
    {
        public const string Listo = "Listo";
        public const string Cancelado = "Cancelado";
        public const string Finalizado = "Finalizado";
        public const string EnCurso = "EnCurso";
    }
}

