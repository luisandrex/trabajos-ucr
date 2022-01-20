using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.MedicalRecords;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.MedicalRecords
{
    public class ListaAntecedenteMap : IEntityTypeConfiguration<ListaAntecedentes>
    {
        public void Configure(EntityTypeBuilder<ListaAntecedentes> builder)
        {
            builder.ToTable("ListaAntecedentes");
            builder.HasKey("Id");
        }
    }
}
