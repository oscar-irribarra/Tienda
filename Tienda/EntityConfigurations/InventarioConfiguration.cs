using System.Data.Entity.ModelConfiguration;
using Tienda.Models;

namespace Tienda.EntityConfigurations
{
    public class InventarioConfiguration : EntityTypeConfiguration<Inventario>
    {
        public InventarioConfiguration()
        {
            ToTable("Inventario");

            HasKey(i => i.Id);

            HasRequired(i => i.Producto)
                .WithMany(i => i.Inventario)
                .HasForeignKey(i => i.ProductoId)
                .WillCascadeOnDelete(false);
        }
    }
}