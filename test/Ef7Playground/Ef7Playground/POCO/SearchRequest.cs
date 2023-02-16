using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[Table("SearchRequests", Schema = "dashboard")]
[Index("Username", Name = "IX_SearchRequests_Username")]
[Table("SearchRequests", Schema = "dashboard")]
public partial class SearchRequest
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(128)]
    public string? Username { get; set; }

    public string? SearchText { get; set; }

    public string? ClickedDeeplink { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
}
