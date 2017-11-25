using System.Data.Entity.ModelConfiguration;
using Tienda.Models;

namespace Tienda.EntityConfigurations
{
    public class DetalleProductoConfiguration : EntityTypeConfiguration<DetalleProducto>
    {
        public DetalleProductoConfiguration()
        {
            ToTable("DetalleProducto");

            HasKey(dp => dp.Id);

            HasRequired(dp => dp.Producto)
                .WithMany(dp => dp.DetalleProducto)
                .HasForeignKey(dp => dp.ProductoId)
                .WillCascadeOnDelete(false);
        }
    }
}