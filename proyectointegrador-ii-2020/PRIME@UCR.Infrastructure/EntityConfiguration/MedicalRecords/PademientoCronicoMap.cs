using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.MedicalRecords
{
    public class PadecimientoCronicoMap : IEntityTypeConfiguration<PadecimientosCronicos>
    {
        public void Configure(EntityTypeBuilder<PadecimientosCronicos> builder)
        {
            builder.ToTable("PadecimientosCronicos");
            builder.HasKey("IdExpediente", "IdListaPadecimiento");

            builder
                .HasOne(e => e.Expediente)
                .WithMany(e => e.PadecimientosCronicos)
                .HasForeignKey(e => e.IdExpediente);

            builder
                .HasOne(e => e.ListaPadecimiento)
                .WithMany(e => e.PadecimientosCronicos)
                .HasForeignKey(e => e.IdListaPadecimiento);

        }
    }
}
