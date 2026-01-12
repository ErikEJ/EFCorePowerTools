using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Dac.Extensions.Prototype;
using Microsoft.SqlServer.Dac.Model;
using RevEng.Core.Abstractions.Metadata;

namespace ErikEJ.EntityFrameworkCore.SqlServer.Scaffolding;

internal static class DacpacModelFactoryHelper
{
        public static List<ModuleParameterTvpColumn> GetTvpColumns(TSqlTableTypeReference tableReference)
        {
            var tvpColumns = new List<ModuleParameterTvpColumn>();

            var tableTypeObject = tableReference.Element;
            if (tableTypeObject != null)
            {
                var columns = tableTypeObject.GetReferenced(TableType.Columns);
                foreach (var column in columns)
                {
#pragma warning disable S6608 // Prefer indexing instead of "Enumerable" methods on types implementing "IList"
                    var dataTypeObjects = column.GetReferenced(TableTypeColumn.DataType);
                    var dataTypeObject = dataTypeObjects.FirstOrDefault();
                    if (dataTypeObject != null && dataTypeObject.Name.HasName)
                    {
                        var dataTypeName = dataTypeObject.Name.Parts.Last();
                        var maxLength = TableTypeColumn.Length.GetValue<int>(column);
                        var precision = TableTypeColumn.Precision.GetValue<int>(column);
                        var isNullable = TableTypeColumn.Nullable.GetValue<bool>(column);

                        tvpColumns.Add(new ModuleParameterTvpColumn
                        {
                            Name = column.Name.Parts.Last(),
                            DataType = dataTypeName,
                            MaxLength = maxLength == 0 ? null : maxLength,
                            Precision = precision == 0 ? null : precision,
                            Nullable = isNullable,
                        });
#pragma warning restore S6608 // Prefer indexing instead of "Enumerable" methods on types implementing "IList"
                }
                }
            }

            return tvpColumns;
        }
}
