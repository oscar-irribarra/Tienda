using System.Data.Entity.ModelConfiguration;
using Tienda.Models;

namespace Tienda.EntityConfigurations
{
    public class DetalleArriendoConfiguration : EntityTypeConfiguration<DetalleArriendo>
    {
        public DetalleArriendoConfiguration()
        {
            ToTable("DetalleArriendo");

            HasKey(da => da.Id);

            HasRequired(da => da.Producto)
                .WithMany(da => da.DetalleArriendo)
                .HasForeignKey(da => da.ProductoId)
                .WillCascadeOnDelete(false);

            HasRequired(da => da.Arriendo)
                .WithMany(da => da.DetalleArriendo)
                .HasForeignKey(da => da.ArriendoId)
                .WillCascadeOnDelete(false);
        }
    }
}