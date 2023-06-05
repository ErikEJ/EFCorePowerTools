﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using ScaffoldingPostgreSqlMultiSchemaTester.Models.dboNs;
using ScaffoldingPostgreSqlMultiSchemaTester.Models.HumanResourcesNs;
using ScaffoldingPostgreSqlMultiSchemaTester.Models.PersonNs;
using ScaffoldingPostgreSqlMultiSchemaTester.Models.ProductionNs;
using ScaffoldingPostgreSqlMultiSchemaTester.Models.PurchasingNs;
using ScaffoldingPostgreSqlMultiSchemaTester.Models.SalesNs;


namespace ScaffoldingPostgreSqlMultiSchemaTester.Models.ProductionNs
{
    /// <summary>
    /// Manufacturing failure reasons lookup table.
    /// </summary>
    public partial class ScrapReason
    {
        public ScrapReason()
        {
            WorkOrder = new HashSet<WorkOrder>();
        }

        /// <summary>
        /// Primary key for ScrapReason records.
        /// </summary>
        public short ScrapReasonId { get; set; }
        /// <summary>
        /// Failure description.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<WorkOrder> WorkOrder { get; set; }
    }
}