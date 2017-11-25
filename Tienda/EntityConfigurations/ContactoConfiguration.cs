using System.Data.Entity.ModelConfiguration;
using Tienda.Models;

namespace Tienda.EntityConfigurations
{
    public class ContactoConfiguration : EntityTypeConfiguration<Contacto>
    {
        public ContactoConfiguration()
        {
            ToTable("Contacto");

            HasKey(m => m.Id);

            HasRequired(m => m.Estado)
                .WithMany(m => m.Contacto)
                .HasForeignKey(m => m.EstadoId)
                .WillCascadeOnDelete(false);

        }
    }
}