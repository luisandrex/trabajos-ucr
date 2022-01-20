using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRIME_UCR.Domain.Models.Appointments;
using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Appointments
{
    public class EspecialidadMedicaMap : IEntityTypeConfiguration<EspecialidadMedica>
    {
        void IEntityTypeConfiguration<EspecialidadMedica>.Configure(EntityTypeBuilder<EspecialidadMedica> builder) {

            builder.HasKey("Nombre");
            
            builder.ToTable(nameof(EspecialidadMedica));     
        }


    }
}
