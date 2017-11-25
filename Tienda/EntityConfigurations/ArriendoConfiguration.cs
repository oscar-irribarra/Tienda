using System.Data.Entity.ModelConfiguration;
using Tienda.Models;

namespace Tienda.EntityConfigurations
{
    public class ArriendoConfiguration : EntityTypeConfiguration<Arriendo>
    {
        public ArriendoConfiguration()
        {
            ToTable("Arriendo");

            HasKey(p => p.Id);

            HasRequired(p => p.Estado)
                .WithMany(p => p.Arriendo)
                .HasForeignKey(p => p.EstadoId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Documento)
                .WithMany(p => p.Arriendo)
                .HasForeignKey(p => p.DocumentoId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Cliente)
                .WithMany(p => p.Arriendo)
                .HasForeignKey(p => p.ClienteId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.Empresa)
                .WithMany(p => p.Arriendo)
                .HasForeignKey(p => p.EmpresaId)
                .WillCascadeOnDelete(false);

            HasRequired(p => p.TipoProducto)
                .WithMany(p => p.Arriendo)
                .HasForeignKey(p => p.TipoProductoId)
                .WillCascadeOnDelete(false);

            HasRequired(x => x.Vendedor)
                .WithMany(x => x.Arriendo)
                .HasForeignKey(x => x.VendedorId)
                .WillCascadeOnDelete(false);

        }
    }
}