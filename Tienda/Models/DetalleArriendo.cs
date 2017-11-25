using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda.Models
{
    public class DetalleArriendo
    {
        public int Id { get; set; }

        [Required]
        public int ArriendoId { get; set; }

        [Required]        
        public int ProductoId  { get; set; }

        [Required]
        public int Cantidad { get; set; }
       
       
        public virtual Producto Producto { get; set; }     
        public virtual Arriendo Arriendo { get; set; }
    }
}