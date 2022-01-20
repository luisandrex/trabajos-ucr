using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.UserAdministration
{
    public class EspecialistaTécnicoMédicoMap : IEntityTypeConfiguration<EspecialistaTécnicoMédico>
    {
        /**
         *  Function:       Used to configure the table of EspecialistaTécnicoMédico of the database with its model.
         *  Requieres:      The EntityTypeBuilder used to configure the entity framework.
         *  Returns:        Nothing.
         */
        public void Configure(EntityTypeBuilder<EspecialistaTécnicoMédico> builder)
        {
            builder.ToTable("EspecialistaTécnicoMédico");
            builder
                .Property(p => p.Cédula)
                .HasColumnName("Cédula")
                .IsRequired();
        }
    }
}
