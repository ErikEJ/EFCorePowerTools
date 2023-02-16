using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[Table("AuditLogs", Schema = "dbo")]
public partial class AuditLog
{
    [Key]
    public long Id { get; set; }

    public int AuditFor { get; set; }

    public int AuditAction { get; set; }

    public int AuditStatus { get; set; }

    public string? DataId { get; set; }

    public string? Message { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? CustomerIdentifier { get; set; }
}
