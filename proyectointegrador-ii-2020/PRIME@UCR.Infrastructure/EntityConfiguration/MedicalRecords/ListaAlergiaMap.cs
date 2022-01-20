using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.MedicalRecords
{
    public class ListaAlergiaMap : IEntityTypeConfiguration<ListaAlergia>
    {
        public void Configure(EntityTypeBuilder<ListaAlergia> builder)
        {
            builder.ToTable("ListaAlergia");
            builder.HasKey("Id");
        }
    }
}
