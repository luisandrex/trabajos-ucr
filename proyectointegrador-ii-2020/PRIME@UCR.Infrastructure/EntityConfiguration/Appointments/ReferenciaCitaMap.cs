using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Appointments
{
    public class ReferenciaCitaMap : IEntityTypeConfiguration<ReferenciaCita>
    {
        void IEntityTypeConfiguration<ReferenciaCita>.Configure(EntityTypeBuilder<ReferenciaCita> builder) {


            builder.HasKey("IdCita1", "IdCita2");

            builder
                .HasOne(e => e.Cita1)
                .WithMany()
                .HasForeignKey(e => e.IdCita1);

            builder
                .HasOne(e => e.Cita2)
                .WithMany()
                .HasForeignKey(e => e.IdCita2);


            builder
                .HasOne(e => e.EspecialidadMedica)
                .WithMany()
                .HasForeignKey(e => e.Especialidad);

            builder.ToTable(nameof(ReferenciaCita));
        }

    }
}
