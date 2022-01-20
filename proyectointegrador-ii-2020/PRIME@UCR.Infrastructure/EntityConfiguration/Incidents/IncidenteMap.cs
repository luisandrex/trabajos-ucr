using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Incidents
{
    public class IncidenteMap : IEntityTypeConfiguration<Incidente>
    {
        public void Configure(EntityTypeBuilder<Incidente> builder)
        {
            builder.ToTable("Incidente");
            builder.HasKey("Codigo");
            builder
                .HasOne(p => p.Origen)
                .WithOne()
                .HasForeignKey<Incidente>(p => p.IdOrigen);
            builder
                .HasOne(p => p.Destino)
                .WithOne()
                .HasForeignKey<Incidente>(p => p.IdDestino);
            builder
                .Property(p => p.Codigo)
                .HasMaxLength(50)
                .HasDefaultValueSql();
            builder
                .Property(p => p.Modalidad);
            builder
                .HasOne<Modalidad>()
                .WithMany(p => p.Incidentes)
                .HasForeignKey(p => p.Modalidad);
            builder
                .HasOne(p => p.UnidadDeTransporte)
                .WithMany()
                .HasForeignKey(p => p.MatriculaTrans);
            builder
                .HasOne(i => i.Cita)
                .WithMany(p => p.Incidentes)
                .HasForeignKey(i => i.CodigoCita);
            builder
                .HasOne<CoordinadorTécnicoMédico>()
                .WithMany()
                .HasForeignKey(i => i.CedulaTecnicoCoordinador);
            builder
                .HasOne<Persona>()
                .WithMany()
                .HasForeignKey(i => i.CedulaRevisor);
        }
    }
}
