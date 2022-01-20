using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models;
using RepoDb;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Incidents
{
    public class CentroMedicoMap : IEntityTypeConfiguration<CentroMedico>
    {
        public void Configure(EntityTypeBuilder<CentroMedico> builder)
        {
            // EFCore
            builder.ToTable("Centro_Medico");
            builder
                .Property(p => p.Id)
                .IsRequired();
            builder.HasKey("Id");
            builder
                .HasOne(p => p.Distrito)
                .WithMany(p => p.CentroMedicos)
                .HasForeignKey(p => p.UbicadoEn);

            // RepoDb
            FluentMapper.Entity<CentroMedico>()
                .Table("Centro_Medico");
        }

    }

}
