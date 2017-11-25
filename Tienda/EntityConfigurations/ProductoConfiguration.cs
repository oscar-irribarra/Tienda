using System.Data.Entity.ModelConfiguration;
using Tienda.Models;

namespace Tienda.EntityConfigurations
{
    public class ProductoConfiguration : EntityTypeConfiguration<Producto>
    {
        public ProductoConfiguration()
        {
            ToTable("Producto");

            HasKey(p => p.Id);

            HasRequired(p => p.Categoria)
                .WithMany(p => p.Producto)
                .HasForeignKey(p => p.CategoriaId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Impuesto)
                .WithMany(p => p.Producto)
                .HasForeignKey(p => p.ImpuestoId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.TipoProducto)
                .WithMany(p => p.Producto)
                .HasForeignKey(p => p.TipoProductoId)
                .WillCascadeOnDelete(false);

        }
    }
}