using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Multimedia
{
    public class CitaMap : IEntityTypeConfiguration<Cita>
    {
        void IEntityTypeConfiguration<Cita>.Configure(EntityTypeBuilder<Cita> builder)
        {
            builder
                .HasKey("Id");

            builder
                .HasOne(c => c.Expediente)
                .WithMany( c => c.Citas)
                .HasForeignKey(c => c.IdExpediente);

            builder.ToTable(nameof(Cita));
        }
    }
}
