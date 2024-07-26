using System;
using System.IO;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Dac;

namespace RevEng.Core.DacpacReport
{
    public class DacpacExtractor
    {
        private readonly SqlConnectionStringBuilder connectionStringBuilder;

        public DacpacExtractor(SqlConnectionStringBuilder connectionStringBuilder)
        {
            ArgumentNullException.ThrowIfNull(connectionStringBuilder);
            this.connectionStringBuilder = connectionStringBuilder;
        }

        public FileInfo ExtractDacpac()
        {
            var extractedPackagePath = Path.Join(Path.GetTempPath(), connectionStringBuilder.InitialCatalog + ".dacpac");

            var services = new DacServices(connectionStringBuilder.ConnectionString);

            var extractOptions = new DacExtractOptions
            {
                CommandTimeout = 300,
                VerifyExtraction = true,
                IgnorePermissions = true,
                IgnoreUserLoginMappings = true,
                IgnoreExtendedProperties = true,
                Storage = DacSchemaModelStorageType.Memory,
            };

            services.Extract(extractedPackagePath, connectionStringBuilder.InitialCatalog, "EF Core Power Tools", new Version(1, 0), extractOptions: extractOptions);

            return new FileInfo(extractedPackagePath);
        }
    }
}
