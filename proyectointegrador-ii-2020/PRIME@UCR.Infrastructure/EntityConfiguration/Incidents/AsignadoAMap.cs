using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models;
using PRIME_UCR.Domain.Models.Incidents;
using PRIME_UCR.Domain.Models.UserAdministration;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Incidents
{
    public class AsignadoAMap : IEntityTypeConfiguration<AsignadoA>
    {
        public void Configure(EntityTypeBuilder<AsignadoA> builder)
        {
            builder.HasOne<EspecialistaTécnicoMédico>()
                .WithMany()
                .HasForeignKey(a => a.CedulaEspecialista);

            builder.HasOne<Incidente>()
                .WithMany()
                .HasForeignKey(a => a.Codigo);
        }
    }
}