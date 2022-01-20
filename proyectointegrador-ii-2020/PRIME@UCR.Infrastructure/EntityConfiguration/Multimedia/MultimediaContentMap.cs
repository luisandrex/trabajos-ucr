using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using PRIME_UCR.Domain.Models;

namespace PRIME_UCR.Infrastructure.EntityConfiguration.Multimedia
{
    public class MultimediaContentMap : IEntityTypeConfiguration<MultimediaContent>
    {

        public void Configure(EntityTypeBuilder<MultimediaContent> builder)
        {
            builder
                .HasKey("Id");

            builder.ToTable(nameof(MultimediaContent));

        }
    }
}
