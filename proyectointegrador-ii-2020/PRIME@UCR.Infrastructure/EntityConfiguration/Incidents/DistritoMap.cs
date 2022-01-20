using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Incidents
{
    public class DistritoMap : IEntityTypeConfiguration<Distrito>
    {
        public void Configure(EntityTypeBuilder<Distrito> builder)
        {
            builder.ToTable("Distrito");
            builder
                .HasOne(p => p.Canton)
                .WithMany(p => p.Distritos)
                .HasForeignKey(p => p.IdCanton);
            builder
                .Property(p => p.Id)
                .IsRequired();
            builder.HasKey("Id");

        }

    }

}
