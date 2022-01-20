using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Incidents
{
    public class InternacionalMap : IEntityTypeConfiguration<Internacional>
    {
        public void Configure(EntityTypeBuilder<Internacional> builder)
        {
            builder.ToTable("Internacional");
            // no key because it is a derived type
            builder
                .HasOne(p => p.Pais)
                .WithMany(p => p.PaisUbicaciones)
                .HasForeignKey(p => p.NombrePais);
        }

    }

}
