using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Appointments
{
    public class SeEspecializaMap : IEntityTypeConfiguration<SeEspecializa>
    {
        void IEntityTypeConfiguration<SeEspecializa>.Configure(EntityTypeBuilder<SeEspecializa> builder) {

            builder.HasKey("CedulaMedico", "NombreEspecialidad");

            builder
                .HasOne(e => e.Especialidad)
                .WithMany(e => e.Especialistas)
                .HasForeignKey(e => e.NombreEspecialidad);

            builder
                .HasOne(e => e.Medico)
                .WithMany(e => e.Especialidades)
                .HasForeignKey(e => e.CedulaMedico);

            builder.ToTable(nameof(SeEspecializa)); 
        }

    }
}
