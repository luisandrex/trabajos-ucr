using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PRIME_UCR.Domain.Models;


namespace PRIME_UCR.Infrastructure.EntityConfiguration.Incidents
{
    public class TrabajaEnMap : IEntityTypeConfiguration<TrabajaEn>
    {
        public void Configure(EntityTypeBuilder<TrabajaEn> builder)
        {
            builder.ToTable("Trabaja_En");

            builder.HasKey(k => new { k.CédulaMédico, k.CentroMedicoId });
            builder
                .HasOne(p => p.Médico)
                .WithMany()
                .HasForeignKey(p => p.CédulaMédico);
            builder
                .HasOne(p => p.CentroMedico)
                .WithMany(p => p.MedicosyCentrosMedicos)
                .HasForeignKey(p => p.CentroMedicoId);
        }
    }
}

