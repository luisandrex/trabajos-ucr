using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;
using RepoDb;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Incidents
{
    public class EstadoIncidenteMap : IEntityTypeConfiguration<EstadoIncidente>
    {
        public void Configure(EntityTypeBuilder<EstadoIncidente> builder)
        {
            builder.ToTable("EstadoIncidente");
            
            builder.HasKey(e => new {IncidenteId = e.CodigoIncidente, e.NombreEstado});

            builder
                .HasOne(e => e.Estado)
                .WithMany(e => e.EstadoIncidentes)
                .HasForeignKey(e => e.NombreEstado);

            builder
                .HasOne<CoordinadorTécnicoMédico>()
                .WithMany()
                .HasForeignKey(i => i.AprobadoPor);

            builder
                .HasOne(e => e.Incidente)
                .WithMany(i => i.EstadoIncidentes)
                .HasForeignKey(e => e.CodigoIncidente);

            builder
                .Property(e => e.FechaHora)
                .HasColumnName("FechaHora");
        }
    }
}