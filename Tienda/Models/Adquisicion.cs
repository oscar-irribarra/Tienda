using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda.Models
{   
    public class Adquisicion
    {
        public Adquisicion()
        {
            DetalleAdquisicion = new HashSet<DetalleAdquisicion>();
        }
        public int Id { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Required]
        public int DocumentoId { get; set; }

        [Required]
        public int ProveedorId { get; set; }

        [Required]
        public int EstadoId { get; set; }

        [Required]
        public int EmpresaId { get; set; }

        [Required]
        public int TipoProductoId { get; set; }
        
        [Required]
        public string VendedorId { get; set; }
        public virtual ApplicationUser Vendedor { get; set; }

        public virtual TipoProducto TipoProducto { get; set; }
        public virtual Proveedor Proveedor { get; set; }       
        public virtual Estado Estado { get; set; }                  
        public virtual Documento Documento { get; set; }
        public virtual Empresa Empresa { get; set; }

        public virtual ICollection<DetalleAdquisicion> DetalleAdquisicion { get; set; }
    }
}

