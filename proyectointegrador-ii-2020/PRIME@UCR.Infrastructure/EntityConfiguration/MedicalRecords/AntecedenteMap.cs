using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.MedicalRecords
{
    public class AntecedenteMap : IEntityTypeConfiguration<Antecedentes>
    {
        public void Configure(EntityTypeBuilder<Antecedentes> builder)
        {
            builder.ToTable("Antecedentes");
            builder.HasKey("IdExpediente", "IdListaAntecedentes");

            builder
                .HasOne(e => e.Expediente)
                .WithMany(e => e.Antecedentes)
                .HasForeignKey(e => e.IdExpediente);

            builder
                .HasOne(e => e.ListaAntecedentes)
                .WithMany(e => e.Antecedentes)
                .HasForeignKey(e => e.IdListaAntecedentes);

        }
    }
}
