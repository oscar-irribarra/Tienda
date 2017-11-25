using System.Data.Entity.ModelConfiguration;
using Tienda.Models;

namespace Tienda.EntityConfigurations
{
    public class DetalleAdquisicionConfiguration : EntityTypeConfiguration<DetalleAdquisicion>
    {
        public DetalleAdquisicionConfiguration()
        {
            ToTable("DetalleAdquisicion");

            HasKey(da => da.Id);

            HasRequired(da => da.Producto)
                .WithMany(da => da.DetalleAdquisicion)
                .HasForeignKey(da => da.ProductoId)
                .WillCascadeOnDelete(false);

            HasRequired(da => da.Adquisicion)
                .WithMany(da => da.DetalleAdquisicion)
                .HasForeignKey(da => da.AdquisicionId)
                .WillCascadeOnDelete(false);
        }
    }
}