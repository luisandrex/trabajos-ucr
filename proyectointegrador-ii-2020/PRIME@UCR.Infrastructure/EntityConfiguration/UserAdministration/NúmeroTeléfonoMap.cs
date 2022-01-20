using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.UserAdministration
{
    public class NúmeroTeléfonoMap : IEntityTypeConfiguration<NúmeroTeléfono>
    {
        /**
        *  Function:       Used to configure the table NúmeroTeléfono of the database with its model.
        *  Requieres:      The EntityTypeBuilder used to configure the entity framework.
        *  Returns:        Nothing.
        */
        public void Configure(EntityTypeBuilder<NúmeroTeléfono> builder)
        {
            builder.ToTable("NúmeroTeléfono");
            builder
                .Property(p => p.CedPersona)
                .HasColumnName("CédulaPersona")
                .IsRequired()
                .HasMaxLength(12);
            builder
                .Property(p => p.NúmeroTelefónico)
                .HasColumnName("Número")
                .IsRequired()
                .HasMaxLength(8);
            builder.HasKey("CedPersona", "NúmeroTelefónico");
            builder
                .HasOne(p => p.Persona)
                .WithMany()
                .HasForeignKey(p => p.CedPersona);
        }
    }
}
