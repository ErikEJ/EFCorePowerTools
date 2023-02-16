using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[Table("CompanyStructures", Schema = "dbo")]
public partial class CompanyStructure
{
    [Key]
    public string Identifier { get; set; } = null!;

    public string? Structure { get; set; }

    public DateTime CreatedAt { get; set; }
}
