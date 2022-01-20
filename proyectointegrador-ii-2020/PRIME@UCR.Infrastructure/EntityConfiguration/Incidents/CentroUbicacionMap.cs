using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models;
using RepoDb;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Incidents
{
    public class CentroUbicacionMap : IEntityTypeConfiguration<CentroUbicacion>
    {
        public void Configure(EntityTypeBuilder<CentroUbicacion> builder)
        {
            // EFCore
            builder.ToTable("Centro_Ubicacion");
            // no key because it is a derived type
            builder
                .Property(p => p.Id)
                .IsRequired();
            builder
                .Property(p => p.CentroMedicoId)
                .HasColumnName("IdCentro");
            builder
                .HasOne(p => p.CentroMedico)
                .WithMany(p => p.UbicacionIncidentes)
                .HasForeignKey(p => p.CentroMedicoId);
            builder
                .Property(p => p.CedulaMedico)
                .HasColumnName("CédulaMédico");
            builder
                .HasOne(p => p.Médico)
                .WithMany()
                .HasForeignKey(p => p.CedulaMedico);
            
            // RepoDb
            FluentMapper.Entity<CentroUbicacion>()
                .Table("Centro_Ubicacion")
                .Column(cu => cu.CentroMedicoId, "IdCentro")
                .Column(cu => cu.CedulaMedico, "CédulaMédico");
        }
    }

}
