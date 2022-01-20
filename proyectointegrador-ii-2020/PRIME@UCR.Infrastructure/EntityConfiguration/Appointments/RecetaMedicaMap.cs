using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.Appointments;
using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Appointments
{
    public class RecetaMedicaMap : IEntityTypeConfiguration<RecetaMedica>
    {
        public void Configure(EntityTypeBuilder<RecetaMedica> builder)
        {
            builder.ToTable("RecetaMedica");
            builder.HasKey("Id");
        }
    }
}
