using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.UserAdministration;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.UserAdministration
{
    public class PermisoMap : IEntityTypeConfiguration<Permiso>
    {
        /**
        *  Function:       Used to configure the table Permiso of the database with its model.
        *  Requieres:      The EntityTypeBuilder used to configure the entity framework.
        *  Returns:        Nothing.
        */
        public void Configure(EntityTypeBuilder<Permiso> builder)
        {
            builder.ToTable("Permiso");
            builder
                .Property(p => p.IDPermiso)
                .HasColumnName("IdPermiso")
                .IsRequired();
            builder.HasKey("IDPermiso");
            builder
                .Property(p => p.DescripciónPermiso)
                .HasColumnName("Descripción_Permiso")
                .HasMaxLength(100);
            builder
                .HasMany(p => p.PerfilesYPermisos)
                .WithOne(p => p.Permiso);

            FluentMapper.Entity<Permiso>().Column((p => p.IDPermiso), "IDPermiso");
            FluentMapper.Entity<Permiso>().Column((p => p.DescripciónPermiso), "Descripción_Permiso");
        }
    }
}
