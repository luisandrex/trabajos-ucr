using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.UserAdministration
{
    public class AdministradorMap : IEntityTypeConfiguration<Administrador>
    {
        /**
         *  Function:       Used to configure the table of Administrador of the database with its model.
         *  Requieres:      The EntityTypeBuilder used to configure the entity framework.
         *  Returns:        Nothing.
         */
        public void Configure(EntityTypeBuilder<Administrador> builder)
        {
            builder.ToTable("Administrador");
            builder
                .Property(p => p.Cédula)
                .HasColumnName("Cédula")
                .IsRequired();
        }
    }
}
