using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Incidents
{
    public class CantonMap : IEntityTypeConfiguration<Canton>
    {
        public void Configure(EntityTypeBuilder<Canton> builder)
        {
            builder.ToTable("Canton");
            builder
                .Property(p => p.Id)
                .IsRequired();
            builder.HasKey("Id");
            builder
                .HasOne(p => p.Provincia)
                .WithMany(p => p.Cantones)
                .HasForeignKey(p => p.NombreProvincia);
        }
    }
}
