using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.CheckLists;
using RepoDb;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.CheckLists
{
    class InstanciaItemMap : IEntityTypeConfiguration<InstanciaItem>
    {
        public void Configure(EntityTypeBuilder<InstanciaItem> builder)
        {
            builder.ToTable("InstanciaItem");
            builder
                .Property(i => i.ItemId)
                .HasColumnName("Id_Item")
                .IsRequired();
            builder
                .Property(i => i.PlantillaId)
                .HasColumnName("Id_Lista")
                .IsRequired();
            builder
                .Property(i => i.IncidentCod)
                .HasColumnName("Codigo_Incidente")
                .IsRequired();
            builder.HasKey(k => new { k.ItemId, k.PlantillaId, k.IncidentCod });
            builder
                .Property(i => i.ItemPadreId)
                .HasColumnName("Id_Item_Padre");
            builder
                .Property(i => i.PlantillaPadreId)
                .HasColumnName("Id_Lista_Padre");
            builder
                .Property(i => i.IncidentCodPadre)
                .HasColumnName("Codigo_Incidente_Padre");
            builder
                .HasOne(i => i.MyItem)
                .WithMany(I => I.Instances)
                .HasForeignKey(i => i.ItemId);
            builder
                .HasOne(i => i.MyFather)
                .WithMany(i => i.SubItems)
                .HasForeignKey(k => new { k.ItemPadreId, k.PlantillaPadreId, k.IncidentCodPadre });

            // RepoDb
            FluentMapper.Entity<InstanciaItem>()
                        .Table("InstanciaItem")
                        .Column(cu => cu.Completado, "Completado")
                        .Column(cu => cu.FechaHoraInicio, "FechaHoraInicio")
                        .Column(cu => cu.ItemId, "Id_Item")
                        .Column(cu => cu.PlantillaId, "Id_Lista")
                        .Column(cu => cu.IncidentCod, "Codigo_Incidente")
                        .Column(cu => cu.ItemPadreId, "Id_Item_Padre")
                        .Column(cu => cu.PlantillaPadreId, "Id_Lista_Padre")
                        .Column(cu => cu.IncidentCodPadre, "Codigo_Incidente_Padre")
                        .Column(cu => cu.FechaHoraFin, "FechaHoraFin");
        }
    }
}
