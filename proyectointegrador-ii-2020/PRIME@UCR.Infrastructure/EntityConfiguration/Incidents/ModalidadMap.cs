using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Incidents
{
    public class ModalidadMap : IEntityTypeConfiguration<Modalidad>
    {
        public void Configure(EntityTypeBuilder<Modalidad> builder)
        {
            builder.ToTable("Modalidad");
            builder
                .Property(p => p.Tipo)
                .IsRequired();
            builder.HasKey("Tipo");
        }
    }
}
