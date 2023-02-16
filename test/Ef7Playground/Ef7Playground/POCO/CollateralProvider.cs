using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[Table("CollateralProviders", Schema = "dbo")]
public partial class CollateralProvider
{
    [Key]
    public string Identifier { get; set; } = null!;

    public string? Structure { get; set; }

    public DateTime CreatedAt { get; set; }
}
