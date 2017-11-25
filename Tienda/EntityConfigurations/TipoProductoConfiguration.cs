using System.Data.Entity.ModelConfiguration;
using Tienda.Models;

namespace Tienda.EntityConfigurations
{
    public class TipoProductoConfiguration : EntityTypeConfiguration<TipoProducto>
    {
        public TipoProductoConfiguration()
        {
            ToTable("TipoProducto");

            HasKey(c => c.Id);

            HasRequired(c => c.Empresa)
                .WithMany(c => c.TipoProducto)
                .HasForeignKey(c => c.EmpresaId)
                .WillCascadeOnDelete(false);
        }
    }
}