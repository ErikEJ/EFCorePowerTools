using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleApp.Models.Configurations;

public partial class MediaTypeConfiguration : IEntityTypeConfiguration<MediaType>
{
    public void Configure(EntityTypeBuilder<MediaType> entity)
    {
        entity.ToTable("MediaType");

        entity.Property(e => e.Name).HasMaxLength(120);

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<MediaType> modelBuilder);
}
