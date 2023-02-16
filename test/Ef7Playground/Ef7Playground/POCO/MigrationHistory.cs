using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models.POCO;

[PrimaryKey("MigrationId", "ContextKey")]
[Table("__MigrationHistory")]
[Table("__MigrationHistory", Schema = "dbo")]
public partial class MigrationHistory
{
    [Key]
    [StringLength(150)]
    public string MigrationId { get; set; } = null!;

    [Key]
    [StringLength(300)]
    public string ContextKey { get; set; } = null!;

    [Required]
    public byte[] Model { get; set; } = null!;

    [Required]
    [StringLength(32)]
    public string ProductVersion { get; set; } = null!;
}
