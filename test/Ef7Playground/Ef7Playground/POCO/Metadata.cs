using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[Table("Metadata", Schema = "dbo")]
public partial class Metadata
{
    [Key]
    public Guid Id { get; set; }

    public string? Data { get; set; }

    [StringLength(255)]
    public string? DataId { get; set; }

    public int DataType { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string? CustomerIdentifier { get; set; }
}
