using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Tienda.Models;

namespace Tienda.EntityConfigurations
{
    public class DetalleCotizacionConfiguration : EntityTypeConfiguration<DetalleCotizacion>
    {
        public DetalleCotizacionConfiguration()
        {
            ToTable("DetalleCotizacion");

            HasKey(x => x.Id);

            HasRequired(x => x.Producto)
                .WithMany(x => x.DetalleCotizacion)
                .HasForeignKey(x => x.ProductoId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Cotizacion)
                .WithMany(x => x.DetalleCotizacion)
                .HasForeignKey(x => x.CotizacionId)
                .WillCascadeOnDelete(false);
        }
    }
}