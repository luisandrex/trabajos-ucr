using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.CheckLists;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.CheckLists
{
    public class CheckListMap : IEntityTypeConfiguration<CheckList>
    {
        // User Story PIG01IIC20-267 LG - Agregar imagen descriptiva a lista de chequeo
        public void Configure(EntityTypeBuilder<CheckList> builder)
        {
            builder.ToTable("CheckList");
            builder.HasKey("Id");
            // Sets NombreImagen to "defaultCheckList.png" as default value
            builder
                .Property(p => p.ImagenDescriptiva)
                .HasDefaultValue("/images/defaultCheckList.svg");
            builder
                .HasMany(p => p.Items)
                .WithOne(p => p.Checklist);
            builder
                .HasOne(p => p.MyType)
                .WithMany(p => p.Lists)
                .HasForeignKey(p => p.Tipo);
        }
    }
}