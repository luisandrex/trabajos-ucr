using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.UserAdministration;
using RepoDb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.UserAdministration
{
    public class PermiteMap : IEntityTypeConfiguration<Permite>
    {
        public void Configure(EntityTypeBuilder<Permite> builder)
        {
            builder.ToTable("Permite");
            builder
                .Property(p => p.IDPerfil)
                .HasColumnName("NombrePerfil")
                .IsRequired();
            builder
                .Property(p => p.IDPermiso)
                .HasColumnName("IdPermiso")
                .IsRequired();
            builder.HasKey(k => new { k.IDPerfil, k.IDPermiso });
            builder
                .HasOne(p => p.Perfil)
                .WithMany(p => p.PerfilesYPermisos)
                .HasForeignKey(p => p.IDPerfil);
            builder
                .HasOne(p => p.Permiso)
                .WithMany(p => p.PerfilesYPermisos)
                .HasForeignKey(p => p.IDPermiso);

            FluentMapper.Entity<Permite>().Column((p => p.IDPerfil), "NombrePerfil");
            FluentMapper.Entity<Permite>().Column((p => p.IDPermiso), "IdPermiso");
        }
    }
}
