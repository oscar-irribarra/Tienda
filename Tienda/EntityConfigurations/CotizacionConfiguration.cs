using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using Tienda.Models;

namespace Tienda.EntityConfigurations
{
    public class CotizacionConfiguration : EntityTypeConfiguration<Cotizacion>
    {
        public CotizacionConfiguration()
        {
            ToTable("Cotizacion");

            HasKey(x => x.Id);

            HasRequired(x => x.Estado)
                .WithMany(x => x.Cotizacion)
                .HasForeignKey(x => x.EstadoId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Empresa)
                .WithMany(x => x.Cotizacion)
                .HasForeignKey(x => x.EmpresaId)
                .WillCascadeOnDelete(false);
                      
            HasRequired(x => x.Cliente)
                .WithMany(x => x.Cotizacion)
                .HasForeignKey(x => x.ClienteId)
                .WillCascadeOnDelete(false);
            
        }
    }
}