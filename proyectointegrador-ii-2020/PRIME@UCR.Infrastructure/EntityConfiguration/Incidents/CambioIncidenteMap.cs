using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;


namespace PRIME_UCR.Infrastructure.EntityConfiguration.Incidents
{
    public class CambioIncidenteMap : IEntityTypeConfiguration<CambioIncidente>
    {
        public void Configure(EntityTypeBuilder<CambioIncidente> builder)
        {
            builder.ToTable("CambioIncidente");

            builder.HasKey(c => new { CambioIncidenteId = c.CodigoIncidente, c.CedFuncionario, c.FechaHora });

            builder
                .HasOne<Funcionario>()
                .WithMany()
                .HasForeignKey(f => f.CedFuncionario);

            builder
                .HasOne(i => i.Incidente)
                .WithMany(i => i.CambioIncidentes)
                .HasForeignKey(i => i.CodigoIncidente);

            builder
                .Property(e => e.UltimoCambio)
                .HasColumnName("UltimoCambio");
            builder
                .Property(e => e.FechaHora)
                .HasColumnName("FechaHora");
        }
    }
}