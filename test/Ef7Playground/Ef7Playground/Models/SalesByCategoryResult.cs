﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable enable

namespace Ef7Playground.Models
{
    public partial class SalesByCategoryResult
    {
        public string ProductName { get; set; } = default!;
        [Column("TotalPurchase", TypeName = "decimal(38,2)")]
        public decimal? TotalPurchase { get; set; }
    }
}
