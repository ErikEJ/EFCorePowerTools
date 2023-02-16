using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[Table("Logs", Schema = "vfauthz")]
[Index("CreatedBy", Name = "IX_Logs_CreatedBy")]
[Table("Logs", Schema = "vfauthz")]
public partial class Log
{
    [Key]
    public int Id { get; set; }

    public int LogType { get; set; }

    public string? Message { get; set; }

    public int? EntityId { get; set; }

    public int CreatedBy { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
