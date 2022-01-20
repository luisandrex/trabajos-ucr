using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models;


namespace PRIME_UCR.Infrastructure.EntityConfiguration.Incidents
{
    public class DocumentacionIncidenteMap : IEntityTypeConfiguration<DocumentacionIncidente>
    {
        public void Configure(EntityTypeBuilder<DocumentacionIncidente> builder)
        {
            builder.ToTable("DocumentacionIncidente");
            builder
                .Property(p => p.Id)
                .IsRequired();
            builder.HasKey("Id");
            builder
                .HasOne(p => p.Incidente)       
                .WithMany()
                .HasForeignKey(p => p.CodigoIncidente);
        }
    }
}


