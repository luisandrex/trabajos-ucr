using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.CheckLists;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.CheckLists
{
    public class TipoListaChequeoMap : IEntityTypeConfiguration<TipoListaChequeo>
    {
        public void Configure(EntityTypeBuilder<TipoListaChequeo> builder)
        {
            builder.ToTable("TipoListaChequeo");
            builder.HasKey("Nombre");
            // Sets NombreImagen to "defaultCheckList.png" as default value
            builder
                .HasMany(t => t.Lists)
                .WithOne(t => t.MyType);
        }
    }
}
