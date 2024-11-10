using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleApp.Models.Configurations;

public partial class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> entity)
    {
        entity.ToTable("Album", tb => tb.HasComment("Album table"));

        entity.HasIndex(e => e.ArtistId, "IFK_AlbumArtistId");

        entity.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(160)
            .HasComment("Title of album");

        entity.HasOne(d => d.Artist).WithMany(p => p.Albums)
            .HasForeignKey(d => d.ArtistId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_AlbumArtistId");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Album> modelBuilder);
}
