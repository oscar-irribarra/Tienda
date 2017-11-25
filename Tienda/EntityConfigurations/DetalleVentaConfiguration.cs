using System.Data.Entity.ModelConfiguration;
using Tienda.Models;

namespace Tienda.EntityConfigurations
{
    public class DetalleVentaConfiguration : EntityTypeConfiguration<DetalleVenta>
    {
        public DetalleVentaConfiguration()
        {
            ToTable("DetalleVenta");

            HasKey(dv => dv.Id);

            HasRequired(dv => dv.Producto)
                .WithMany(dv => dv.DetalleVenta)
                .HasForeignKey(dv => dv.ProductoId)
                .WillCascadeOnDelete(false);

            HasRequired(dv => dv.Venta)
                .WithMany(dv => dv.DetalleVenta)
                .HasForeignKey(dv => dv.VentaId)
                .WillCascadeOnDelete(false);
        }
    }
}