# Use DataAnnotation attributes to configure the model

## Why

Some prefer to have data annotation attributes on their entity classes rather than using then fluent configuration in OnModelCreating.

## Before

`efpt.config.json`

```json
   "UseFluentApiOnly": true,
```

`NorthwindContext.cs`

```csharp
    modelBuilder.Entity<Shipper>(entity =>
        {
            entity.Property(e => e.ShipperId)
                .HasColumnOrder(0)
                .HasColumnName("ShipperID");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(40)
                .HasColumnOrder(1);
            entity.Property(e => e.Phone)
                .HasMaxLength(24)
                .HasColumnOrder(2);
        });

```

`Shipper.cs`

```csharp
public partial class Shipper
{
    public int ShipperId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
```

## After

`efpt.config.json`

```json
   "UseFluentApiOnly": false,
```

`NorthwindContext.cs`

```csharp
    modelBuilder.Entity<Shipper>(entity =>
    {
        entity.Property(e => e.ShipperId).HasColumnOrder(0);
        entity.Property(e => e.CompanyName).HasColumnOrder(1);
        entity.Property(e => e.Phone).HasColumnOrder(2);
    });
```

`Shipper.cs`

```csharp
public partial class Shipper
{
    [Key]
    [Column("ShipperID")]
    public int ShipperId { get; set; }

    [StringLength(40)]
    public string CompanyName { get; set; } = null!;

    [StringLength(24)]
    public string? Phone { get; set; }

    [InverseProperty("ShipViaNavigation")]
    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
```
