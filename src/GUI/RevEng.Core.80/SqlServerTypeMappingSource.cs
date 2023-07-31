using System;
using System.Data;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace RevEng.Core
{
    public class SqlServerTypeMappingSource : Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerTypeMappingSource
    {
        private readonly bool useDateOnlyTimeOnly;

        public SqlServerTypeMappingSource(TypeMappingSourceDependencies dependencies, RelationalTypeMappingSourceDependencies relationalDependencies, ISqlServerSingletonOptions singletonOptions, bool useDateOnlyTimeOnly)
            : base(dependencies, relationalDependencies, singletonOptions)
        {
            this.useDateOnlyTimeOnly = useDateOnlyTimeOnly;
        }

        protected override RelationalTypeMapping FindMapping(in RelationalTypeMappingInfo mappingInfo)
        {
            if (!useDateOnlyTimeOnly && (mappingInfo.StoreTypeName == "date"))
            {
                return new SqlServerDateTimeTypeMapping("date", DbType.Date);
            }

            if (!useDateOnlyTimeOnly && (mappingInfo.StoreTypeName == "time"))
            {
                return new SqlServerTimeSpanTypeMapping("time");
            }

            return base.FindMapping(mappingInfo);
        }
    }
}
