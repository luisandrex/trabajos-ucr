using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.CheckLists;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.CheckLists
{
    public class ItemMap : IEntityTypeConfiguration<Item>
    {
        // User Story PIG01IIC20-122 LG - Agregar imagen descriptiva a lista de chequeo
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Item");
            builder.HasKey("Id");
            // Sets NombreImagen to "defaultCheckList.png" as default value
            builder
                .Property(p => p.ImagenDescriptiva)
                .HasDefaultValue("/images/defaultCheckList.svg");
            builder
                .HasOne(p => p.Checklist)
                .WithMany(p => p.Items)
                .HasForeignKey(p => p.IDLista)
                .OnDelete(DeleteBehavior.Cascade);
            builder
                .HasOne(p => p.ItemList)
                .WithMany(p => p.SubItems)
                .HasForeignKey(p => p.IDSuperItem);
            builder
                .HasMany(p => p.Instances)
                .WithOne(p => p.MyItem);
        }
    }
}
