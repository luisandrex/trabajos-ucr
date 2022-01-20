using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.UserAdministration
{
    public class GerenteMédicoMap : IEntityTypeConfiguration<GerenteMédico>
    {
        /**
        *  Function:       Used to configure the table of GerenteMédico of the database with its model.
        *  Requieres:      The EntityTypeBuilder used to configure the entity framework.
        *  Returns:        Nothing.
        */
        public void Configure(EntityTypeBuilder<GerenteMédico> builder)
        {
            builder.ToTable("GerenteMédico");
            builder
                .Property(p => p.Cédula)
                .HasColumnName("Cédula")
                .IsRequired();
        }
    }
}
