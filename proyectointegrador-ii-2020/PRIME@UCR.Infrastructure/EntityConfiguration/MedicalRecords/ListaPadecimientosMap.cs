using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.MedicalRecords
{
    public class ListaPadecimientosMap : IEntityTypeConfiguration<ListaPadecimiento>
    {
        public void Configure(EntityTypeBuilder<ListaPadecimiento> builder)
        {
            builder.ToTable("ListaPadecimiento");
            builder.HasKey("Id");
        }
    }
}


