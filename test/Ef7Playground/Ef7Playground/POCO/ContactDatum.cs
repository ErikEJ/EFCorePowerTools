using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models2.POCO;

[Index("ContactName", Name = "IX_ContactData_ContactName")]
[Index("CustomerIdentifier", Name = "IX_ContactData_CustomerIdentifier")]
[Table("ContactData", Schema = "dbo")]
public partial class ContactDatum
{
    [Key]
    public Guid Id { get; set; }

    public string? Email { get; set; }

    public string? CustomerIdentifier { get; set; }

    public DateTime? ManualExpirationDate { get; set; }

    [StringLength(300)]
    public string? ContactName { get; set; }

    public int FinalClassification { get; set; }

    public DateTime? FinalExpirationDate { get; set; }
}
