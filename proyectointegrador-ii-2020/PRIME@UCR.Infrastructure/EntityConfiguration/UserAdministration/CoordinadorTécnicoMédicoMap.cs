using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.UserAdministration
{
    public class CoordinadorTécnicoMédicoMap : IEntityTypeConfiguration<CoordinadorTécnicoMédico>
    {
        /**
         *  Function:       Used to configure the table of CoordinadorTécnicoMédico of the database with its model.
         *  Requieres:      The EntityTypeBuilder used to configure the entity framework.
         *  Returns:        Nothing.
         */
        public void Configure(EntityTypeBuilder<CoordinadorTécnicoMédico> builder)
        {
            builder.ToTable("CoordinadorTécnicoMédico");
            builder
                .Property(p => p.Cédula)
                .HasColumnName("Cédula")
                .IsRequired();
        }
    }
}
