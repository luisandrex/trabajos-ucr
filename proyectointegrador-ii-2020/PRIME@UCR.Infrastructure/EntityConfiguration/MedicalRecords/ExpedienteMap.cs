using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.MedicalRecords
{
    public class ExpedienteMap : IEntityTypeConfiguration<Expediente>
    {
        public void Configure(EntityTypeBuilder<Expediente> builder)
        {
            builder.ToTable("Expediente");
            builder.HasKey("Id");            

            builder
                .HasOne(e => e.Paciente)
                .WithOne()
                .HasForeignKey<Expediente>(e => e.CedulaPaciente);

            builder
                .HasOne(e => e.Medico)
                .WithMany()
                .HasForeignKey(e => e.CedulaMedicoDuenno);

            builder
                .HasMany(e => e.Citas)
                .WithOne(e => e.Expediente);
        }
    }
}
