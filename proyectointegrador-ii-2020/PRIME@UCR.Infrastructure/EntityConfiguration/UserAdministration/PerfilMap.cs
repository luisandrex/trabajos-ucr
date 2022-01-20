using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.UserAdministration
{
    public class PerfilMap : IEntityTypeConfiguration<Perfil>
    {
        /**
        *  Function:       Used to configure the table Perfil of the database with its model.
        *  Requieres:      The EntityTypeBuilder used to configure the entity framework.
        *  Returns:        Nothing.
        */
        public void Configure(EntityTypeBuilder<Perfil> builder)
        {
            builder.ToTable("Perfil");
            builder
                .Property(p => p.NombrePerfil)
                .IsRequired()
                .HasMaxLength(60);
            builder.HasKey("NombrePerfil");
            builder
                .HasMany(p => p.PerfilesYPermisos)
                .WithOne(p => p.Perfil);
            builder
                .HasMany(p => p.UsuariosYPerfiles)
                .WithOne(p => p.Perfil);
            builder
                .HasMany(p => p.FuncionariosYPerfiles)
                .WithOne(p => p.Perfil);
        }
    }
}
