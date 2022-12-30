using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Ef7Playground.Models.POCO;

[PrimaryKey("CustomerId", "CustomerTypeId")]
[Table("CustomerCustomerDemo")]
[Table("CustomerCustomerDemo", Schema = "dbo")]
public partial class CustomerCustomerDemo
{
    [Key]
    [Column("CustomerID")]
    [StringLength(5)]
    public string CustomerId { get; set; } = null!;

    [Key]
    [Column("CustomerTypeID")]
    [StringLength(10)]
    public string CustomerTypeId { get; set; } = null!;
}
