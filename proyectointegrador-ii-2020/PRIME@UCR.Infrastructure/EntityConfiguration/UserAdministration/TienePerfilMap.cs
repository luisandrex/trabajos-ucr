using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.UserAdministration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.UserAdministration
{
    public class TienePerfilMap : IEntityTypeConfiguration<TienePerfil>
    {
        public void Configure(EntityTypeBuilder<TienePerfil> builder)
        {
            builder.ToTable("TienePerfil");
            builder
                .Property(p => p.CedFuncionario)
                .HasColumnName("CédulaFuncionario")
                .IsRequired();
            builder
                .Property(p => p.IDPerfil)
                .HasColumnName("NombrePerfil")
                .IsRequired();
            builder.HasKey(k => new { k.CedFuncionario, k.IDPerfil });
            builder
                .HasOne(p => p.Funcionario)
                .WithMany(p => p.PerfilesDelFuncionario)
                .HasForeignKey(p => p.CedFuncionario);
            builder
                .HasOne(p => p.Perfil)
                .WithMany(p => p.FuncionariosYPerfiles)
                .HasForeignKey(p => p.IDPerfil);
        }
    }
}
