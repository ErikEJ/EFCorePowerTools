using EFCorePowerTools.Locales;
using System.ComponentModel.DataAnnotations;

namespace EFCorePowerTools.Handlers.Compare
{
    public enum CompareAttributes
    {
        [Display(Name = "")]
        NotSet,
        [Display(Name = "Anything", ResourceType = typeof(CompareLocale))]
        MatchAnything,
        [Display(Name = "ColumnName", ResourceType = typeof(CompareLocale))]
        ColumnName,
        [Display(Name = "ColumnType", ResourceType = typeof(CompareLocale))]
        ColumnType,
        [Display(Name = "Nullability", ResourceType = typeof(CompareLocale))]
        Nullability,
        [Display(Name = "SqlDefaultValue", ResourceType = typeof(CompareLocale))]
        DefaultValueSql,
        [Display(Name = "SqlComputedColumn", ResourceType = typeof(CompareLocale))]
        ComputedColumnSql,
        [Display(Name = "PersistentComputedColumn", ResourceType = typeof(CompareLocale))]
        PersistentComputedColumn,
        [Display(Name = "ValueGenerated", ResourceType = typeof(CompareLocale))]
        ValueGenerated,
        [Display(Name = "TableName", ResourceType = typeof(CompareLocale))]
        TableName,
        [Display(Name = "PrimaryKey", ResourceType = typeof(CompareLocale))]
        PrimaryKey,
        [Display(Name = "ConstraintName", ResourceType = typeof(CompareLocale))]
        ConstraintName,
        [Display(Name = "IndexConstraintName", ResourceType = typeof(CompareLocale))]
        IndexConstraintName,
        [Display(Name = "Unique", ResourceType = typeof(CompareLocale))]
        Unique,
        [Display(Name = "DeleteBehabior", ResourceType = typeof(CompareLocale))]
        DeleteBehavior,
        [Display(Name = "NotMappedToDatabase", ResourceType = typeof(CompareLocale))]
        NotMappedToDatabase
    }
}