using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.UserAdministration
{
    public class PersonaMap : IEntityTypeConfiguration<Persona>
    {
        /**
        *  Function:       Used to configure the table Permiso of the database with its model.
        *  Requieres:      The EntityTypeBuilder used to configure the entity framework.
        *  Returns:        Nothing.
        */
        public void Configure(EntityTypeBuilder<Persona> builder)
        {
            builder.ToTable("Persona");
            builder
                .Property(p => p.Cédula)
                .HasColumnName("Cédula")
                .IsRequired()
                .HasMaxLength(12);
            builder.HasKey(p => p.Cédula);
            builder
                .Property(p => p.Nombre)
                .HasColumnName("Nombre")
                .IsRequired()
                .HasMaxLength(20);
            builder
                .Property(p => p.PrimerApellido)
                .HasColumnName("PrimerApellido")
                .IsRequired()
                .HasMaxLength(20);
            builder
                .Property(p => p.SegundoApellido)
                .HasColumnName("SegundoApellido")
                .HasMaxLength(20);
            builder
                .Property(p => p.Sexo)
                .HasColumnName("Sexo")
                .HasMaxLength(1);
            builder
                .Property(p => p.FechaNacimiento)
                .HasColumnName("FechaNacimiento")
                .HasColumnType("Date");
        }
    }
}
