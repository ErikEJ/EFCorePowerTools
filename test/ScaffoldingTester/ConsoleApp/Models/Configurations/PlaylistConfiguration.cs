﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleApp.Models.Configurations;

public partial class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
{
    public void Configure(EntityTypeBuilder<Playlist> entity)
    {
        entity.ToTable("Playlist");

        entity.Property(e => e.Name).HasMaxLength(120);

        entity.HasMany(d => d.Tracks).WithMany(p => p.Playlists)
            .UsingEntity<Dictionary<string, object>>(
                "PlaylistTrack",
                r => r.HasOne<Track>().WithMany()
                        .HasForeignKey("TrackId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PlaylistTrackTrackId"),
                l => l.HasOne<Playlist>().WithMany()
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PlaylistTrackPlaylistId"),
                j =>
                {
                    j.HasKey("PlaylistId", "TrackId").IsClustered(false);
                    j.ToTable("PlaylistTrack");
                    j.HasIndex(new[] { "TrackId" }, "IFK_PlaylistTrackTrackId");
                    });

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Playlist> modelBuilder);
}
