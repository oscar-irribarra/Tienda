using System.Data.Entity.ModelConfiguration;
using Tienda.Models;

namespace Tienda.EntityConfigurations
{
    public class VentaConfiguration : EntityTypeConfiguration<Venta>
    {
        public VentaConfiguration()
        {
            ToTable("Venta");

            HasKey(p => p.Id);

            HasRequired(p => p.Estado)
                .WithMany(p => p.Venta)
                .HasForeignKey(p => p.EstadoId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Empresa)
                .WithMany(p => p.Venta)
                .HasForeignKey(p => p.EmpresaId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Documento)
                .WithMany(p => p.Venta)
                .HasForeignKey(p => p.DocumentoId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Cliente)
                .WithMany(p => p.Venta)
                .HasForeignKey(p => p.ClienteId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.TipoProducto)
                .WithMany(p => p.Venta)
                .HasForeignKey(p => p.TipoProductoId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Vendedor)
                .WithMany(x => x.Venta)
                .HasForeignKey(x => x.VendedorId)
                .WillCascadeOnDelete(false);

        }
    }
}