using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Incidents
{
    public class DomicilioMap : IEntityTypeConfiguration<Domicilio>
    {
        public void Configure(EntityTypeBuilder<Domicilio> builder)
        {
            builder.ToTable("Domicilio");
            // no key because it is a derived type
            builder
                .HasOne(p => p.Distrito)
                .WithMany(p => p.Domicilios)
                .HasForeignKey(p => p.DistritoId);
            builder
                .Property(p => p.Id)
                .IsRequired();
        }

    }

}
