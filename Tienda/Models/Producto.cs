using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Tienda.Models
{    
    public class Producto
    {
        public Producto()
        {
            Inventario = new HashSet<Inventario>();
            DetalleVenta = new HashSet<DetalleVenta>();
            DetalleArriendo = new HashSet<DetalleArriendo>();
            DetalleAdquisicion = new HashSet<DetalleAdquisicion>();
            DetalleProducto = new HashSet<DetalleProducto>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [DisplayFormat(DataFormatString = "{0:c0}")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [Index("Producto_Barcode_Index", IsUnique = true)]
        [Display(Name = "Codigo")]
        public int Barcode { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        public int ImpuestoId { get; set; }

        [Required(ErrorMessage = "El campo {0} es Obligatorio")]
        [Display(Name = "Negocio")]
        public int TipoProductoId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha Vencimiento")]
        public DateTime? FechaVencimiento { get; set; }

        public virtual TipoProducto TipoProducto { get; set; }        
        public virtual Categoria Categoria { get; set; }      
        public virtual Impuesto Impuesto { get; set; }

       

        public virtual ICollection<Inventario> Inventario { get; set; }        
        public virtual ICollection<DetalleVenta> DetalleVenta { get; set; }
        public virtual ICollection<DetalleAdquisicion> DetalleAdquisicion { get; set; }
        public virtual ICollection<DetalleProducto> DetalleProducto { get; set; }
        public virtual ICollection<DetalleArriendo> DetalleArriendo { get; set; }
    }
}

