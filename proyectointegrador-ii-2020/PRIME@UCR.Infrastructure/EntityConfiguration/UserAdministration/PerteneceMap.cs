using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.UserAdministration;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.UserAdministration
{
    public class PerteneceMap : IEntityTypeConfiguration<Pertenece>
    {
        public void Configure(EntityTypeBuilder<Pertenece> builder)
        {
            builder.ToTable("Pertenece");
            builder
                .Property(p => p.IDPerfil)
                .HasColumnName("NombrePerfil")
                .IsRequired();
            builder
                .Property(p => p.IDUsuario)
                .HasColumnName("IdUsuario")
                .IsRequired();
            builder.HasKey(p => new { p.IDPerfil, p.IDUsuario });
            builder
                .HasOne(p => p.Perfil)
                .WithMany(p => p.UsuariosYPerfiles)
                .HasForeignKey(p => p.IDPerfil);
            builder
                .HasOne(p => p.Usuario)
                .WithMany(p => p.UsuariosYPerfiles)
                .HasForeignKey(p => p.IDUsuario);

            FluentMapper.Entity<Pertenece>().Column((p => p.IDPerfil), "NombrePerfil");
            FluentMapper.Entity<Pertenece>().Column((p => p.IDUsuario), "IdUsuario");
        }
    }
}
