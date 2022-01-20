using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Appointments
{
    public class PoseeRecetaMap : IEntityTypeConfiguration<PoseeReceta>
    {
        public void Configure(EntityTypeBuilder<PoseeReceta> builder)
        {
            builder.ToTable("PoseeReceta");
            builder.HasKey("IdRecetaMedica", "IdCitaMedica");

            builder
            .HasOne(e => e.RecetaMedica)
            .WithMany(e => e.Recetas)
            .HasForeignKey(e => e.IdRecetaMedica);


            builder
            .HasOne(e => e.CitaMedica)
            .WithMany(e => e.Recetas)
            .HasForeignKey(e => e.IdCitaMedica);

        }
    }
}
