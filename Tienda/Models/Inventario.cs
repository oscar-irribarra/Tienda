using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tienda.Models
{    
    public class Inventario
    {       
        public int Id { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [Required]
        public int Stock { get; set; }
       
        public virtual Producto Producto { get; set; }
    }
}

