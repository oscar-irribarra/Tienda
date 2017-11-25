using System.Data.Entity.ModelConfiguration;
using Tienda.Models;

namespace Tienda.EntityConfigurations
{
    public class AdquisicionConfiguration : EntityTypeConfiguration<Adquisicion>
    {
        public AdquisicionConfiguration()
        {
            ToTable("Adquisicion");

            HasKey(a => a.Id);

            HasRequired(a => a.Estado)
                .WithMany(a => a.Adquisicion)
                .HasForeignKey(a => a.EstadoId)
                .WillCascadeOnDelete(false);

            HasRequired(a => a.Proveedor)
                .WithMany(a => a.Adquisicion)
                .HasForeignKey(a => a.ProveedorId)
                .WillCascadeOnDelete(false);

            HasRequired(a => a.Documento)
                .WithMany(a => a.Adquisicion)
                .HasForeignKey(a => a.DocumentoId)
                .WillCascadeOnDelete(false);

            HasRequired(a => a.Empresa)
                .WithMany(a => a.Adquisicion)
                .HasForeignKey(a => a.EmpresaId)
                .WillCascadeOnDelete(false);

            HasRequired(a => a.TipoProducto)
                .WithMany(a => a.Adquisicion)
                .HasForeignKey(a => a.TipoProductoId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Vendedor)
                .WithMany(x => x.Adquisicion)
                .HasForeignKey(x => x.VendedorId)
                .WillCascadeOnDelete(false);
        }
    }
}