using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Appointments
{
    public class EstadoCitaMedicaMap : IEntityTypeConfiguration<EstadoCitaMedica>
    {
        public void Configure(EntityTypeBuilder<EstadoCitaMedica> builder) {

            builder.HasKey("Id");

            builder.ToTable(nameof(EstadoCitaMedica));

        }

    }
}
