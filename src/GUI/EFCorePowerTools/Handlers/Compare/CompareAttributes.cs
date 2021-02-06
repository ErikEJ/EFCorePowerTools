using System.ComponentModel.DataAnnotations;

namespace EFCorePowerTools.Handlers.Compare
{
    public enum CompareAttributes
    {
        [Display(Name = "")]
        NotSet,
        [Display(Name = "Anything")]
        MatchAnything,
        [Display(Name = "Column name")]
        ColumnName,
        [Display(Name = "Column type")]
        ColumnType,
        [Display(Name = "Nullability")]
        Nullability,
        [Display(Name = "Sql default value")]
        DefaultValueSql,
        [Display(Name = "Sql computed column")]
        ComputedColumnSql,
        [Display(Name = "Persistent computed column")]
        PersistentComputedColumn,
        [Display(Name = "Value generated")]
        ValueGenerated,
        [Display(Name = "Table name")]
        TableName,
        [Display(Name = "Primary key")]
        PrimaryKey,
        [Display(Name = "Constraint name")]
        ConstraintName,
        [Display(Name = "Index constraint name")]
        IndexConstraintName,
        [Display(Name = "Unique")]
        Unique,
        [Display(Name = "Delete behabior")]
        DeleteBehavior,
        [Display(Name = "Not mapped to database")]
        NotMappedToDatabase
    }
}