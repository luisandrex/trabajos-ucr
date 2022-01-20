using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Appointments
{
    public class CitaMedicaMap : IEntityTypeConfiguration<CitaMedica>
    {
        public void Configure(EntityTypeBuilder<CitaMedica> builder)
        {
            builder.HasKey("Id");

            builder
            .HasOne(e => e.Cita)
            .WithMany(e => e.CitasMedicas)
            .HasForeignKey(e => e.CitaId);

            builder
            .HasOne(e => e.Medico)
            .WithMany(e => e.CitasMedicas)
            .HasForeignKey(e => e.CedMedicoAsignado);

            builder.ToTable(nameof(CitaMedica));
        }
    }
}
