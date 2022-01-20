using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.UserAdministration
{
    public class AdministradorCentroDeControlMap : IEntityTypeConfiguration<AdministradorCentroDeControl>
    {
        /**
         *  Function:       Used to configure the table of AdministradorCentroDeControl of the database with its model.
         *  Requieres:      The EntityTypeBuilder used to configure the entity framework.
         *  Returns:        Nothing.
         */
        public void Configure(EntityTypeBuilder<AdministradorCentroDeControl> builder)
        {
            builder.ToTable("AdministradorCentroDeControl");
            builder
                .Property(p => p.Cédula)
                .HasColumnName("Cédula")
                .IsRequired();
        }
    }
}
