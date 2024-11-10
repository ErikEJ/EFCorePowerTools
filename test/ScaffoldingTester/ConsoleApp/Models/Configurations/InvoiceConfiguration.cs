using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsoleApp.Models.Configurations;

public partial class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> entity)
    {
        entity.ToTable("Invoice");

        entity.HasIndex(e => e.CustomerId, "IFK_InvoiceCustomerId");

        entity.Property(e => e.BillingAddress).HasMaxLength(70);
        entity.Property(e => e.BillingCity).HasMaxLength(40);
        entity.Property(e => e.BillingCountry).HasMaxLength(40);
        entity.Property(e => e.BillingPostalCode).HasMaxLength(10);
        entity.Property(e => e.BillingState).HasMaxLength(40);
        entity.Property(e => e.InvoiceDate).HasColumnType("datetime");
        entity.Property(e => e.Total).HasColumnType("numeric(10, 2)");

        entity.HasOne(d => d.Customer).WithMany(p => p.Invoices)
            .HasForeignKey(d => d.CustomerId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_InvoiceCustomerId");

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<Invoice> modelBuilder);
}
