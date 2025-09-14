using System.Data;
#if CORE100
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
#endif
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace RevEng.Core
{
    public class SqlServerTypeMappingSource : Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal.SqlServerTypeMappingSource
    {
        private readonly bool useDateOnlyTimeOnly;

#if CORE100
        public SqlServerTypeMappingSource(TypeMappingSourceDependencies dependencies, RelationalTypeMappingSourceDependencies relationalDependencies, ISqlServerSingletonOptions sqlServerSingletonOptions, bool useDateOnlyTimeOnly)
            : base(dependencies, relationalDependencies, sqlServerSingletonOptions)
        {
            this.useDateOnlyTimeOnly = useDateOnlyTimeOnly;
        }

#else
        public SqlServerTypeMappingSource(TypeMappingSourceDependencies dependencies, RelationalTypeMappingSourceDependencies relationalDependencies, bool useDateOnlyTimeOnly)
            : base(dependencies, relationalDependencies)
        {
            this.useDateOnlyTimeOnly = useDateOnlyTimeOnly;
        }

#endif
        protected override RelationalTypeMapping FindMapping(in RelationalTypeMappingInfo mappingInfo)
        {
            if (!useDateOnlyTimeOnly && (mappingInfo.StoreTypeNameBase == "date"))
            {
                return new SqlServerDateTimeTypeMapping("date", DbType.Date);
            }

            if (!useDateOnlyTimeOnly && (mappingInfo.StoreTypeNameBase == "time"))
            {
                return new SqlServerTimeSpanTypeMapping("time");
            }

            return base.FindMapping(mappingInfo);
        }
    }
}