using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.Models;

public partial class ChinookContext : DbContext
{
    public ChinookContext(DbContextOptions<ChinookContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceLine> InvoiceLines { get; set; }

    public virtual DbSet<MediaType> MediaTypes { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Configurations.AlbumConfiguration());

        modelBuilder.ApplyConfiguration(new Configurations.ArtistConfiguration());

        modelBuilder.ApplyConfiguration(new Configurations.CustomerConfiguration());

        modelBuilder.ApplyConfiguration(new Configurations.EmployeeConfiguration());

        modelBuilder.ApplyConfiguration(new Configurations.GenreConfiguration());

        modelBuilder.ApplyConfiguration(new Configurations.InvoiceConfiguration());

        modelBuilder.ApplyConfiguration(new Configurations.InvoiceLineConfiguration());

        modelBuilder.ApplyConfiguration(new Configurations.MediaTypeConfiguration());

        modelBuilder.ApplyConfiguration(new Configurations.PlaylistConfiguration());

        modelBuilder.ApplyConfiguration(new Configurations.TrackConfiguration());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
