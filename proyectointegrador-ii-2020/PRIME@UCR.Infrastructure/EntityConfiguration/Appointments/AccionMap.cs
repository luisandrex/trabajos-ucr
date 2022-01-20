using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Appointments
{
    public class AccionMap : IEntityTypeConfiguration<Accion>
    {
        void IEntityTypeConfiguration<Accion>.Configure(EntityTypeBuilder<Accion> builder)
        {
            builder
                .HasKey(a => new { a.CitaId, a.MultContId, a.NombreAccion });

            builder.ToTable(nameof(Accion));

            builder
                .HasOne(a => a.Cita)
                .WithMany(c => c.Acciones)
                .HasForeignKey(a => a.CitaId);

            builder
                .HasOne(a => a.TipoAccion)
                .WithMany()
                .HasForeignKey(a => a.NombreAccion);

            builder
                .HasOne(a => a.MultimediaContent)
                .WithMany()
                .HasForeignKey(a => a.MultContId);
        }
    }
}
