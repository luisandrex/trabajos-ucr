using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.UserAdministration
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        /**
        *  Function:       Used to configure the table Usuario of the database with its model.
        *  Requieres:      The EntityTypeBuilder used to configure the entity framework.
        *  Returns:        Nothing.
        */
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuario");
            builder
                .Property(p => p.CedPersona)
                .HasColumnName("CédulaPersona")
                .IsRequired();
            builder
                .Property(p => p.Id)
                .IsRequired();
            builder
                .HasOne(p => p.Persona)
                .WithOne()
                .HasForeignKey<Usuario>(p => p.CedPersona);
            builder
                .HasMany(p => p.UsuariosYPerfiles)
                .WithOne(p => p.Usuario);
        }
    }
}
