using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[Index("CustomerIdentifier", Name = "IX_DataClassifications_CustomerIdentifier")]
[Table("DataClassifications", Schema = "dbo")]
public partial class DataClassification
{
    [Key]
    public long Id { get; set; }

    public string? CustomerIdentifier { get; set; }

    public int Topic { get; set; }

    public string? DataId { get; set; }

    public string? Metadata { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public int Classification { get; set; }
}
