using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleApp.Models.Configurations;

public partial class InvoiceLineConfiguration : IEntityTypeConfiguration<InvoiceLine>
{
    public void Configure(EntityTypeBuilder<InvoiceLine> entity)
    {
        entity.ToTable("InvoiceLine");

        entity.HasIndex(e => e.InvoiceId, "IFK_InvoiceLineInvoiceId");

        entity.HasIndex(e => e.TrackId, "IFK_InvoiceLineTrackId");

        entity.Property(e => e.UnitPrice).HasColumnType("numeric(10, 2)");

        entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceLines)
            .HasForeignKey(d => d.InvoiceId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_InvoiceLineInvoiceId");

        entity.HasOne(d => d.Track).WithMany(p => p.InvoiceLines)
            .HasForeignKey(d => d.TrackId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_InvoiceLineTrackId");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<InvoiceLine> modelBuilder);
}
