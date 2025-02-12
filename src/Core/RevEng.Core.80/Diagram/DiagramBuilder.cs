using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using RevEng.Common;
using RevEng.Common.Dab;

namespace RevEng.Core.Diagram
{
    public class DiagramBuilder
    {
        private readonly ServiceProvider serviceProvider;
        private readonly DataApiBuilderOptions dataApiBuilderOptions;
        private readonly List<string> schemas = Enumerable.Empty<string>().ToList();

        public DiagramBuilder(DataApiBuilderOptions dataApiBuilderCommandOptions, List<string> schemas)
        {
            dataApiBuilderOptions = dataApiBuilderCommandOptions;

            dataApiBuilderOptions.ConnectionString = dataApiBuilderOptions.ConnectionString.ApplyDatabaseType(dataApiBuilderOptions.DatabaseType);

            var options = new ReverseEngineerCommandOptions
            {
                DatabaseType = dataApiBuilderOptions.DatabaseType,
                ConnectionString = dataApiBuilderOptions.ConnectionString,
                Dacpac = dataApiBuilderOptions.Dacpac,
                MergeDacpacs = dataApiBuilderOptions.MergeDacpacs,
            };

            serviceProvider = new ServiceCollection().AddEfpt(options, [], [], []).BuildServiceProvider();
        }

        public string GetDgmlFileName()
        {
            var fileName = Path.Join(Path.GetTempPath(), Path.GetRandomFileName() + ".dgml");
            var model = GetModelInternal();

            var creator = new DatabaseModelToDgml(model, fileName);

            creator.CreateDgml();

            return fileName;
        }

        private DatabaseModel GetModelInternal()
        {
            var dbModelFactory = serviceProvider.GetService<IDatabaseModelFactory>();

            var dbModelOptions = new DatabaseModelFactoryOptions(schemas: schemas);

            var dbModel = dbModelFactory!.Create(dataApiBuilderOptions.Dacpac ?? dataApiBuilderOptions.ConnectionString, dbModelOptions);

            return dbModel;
        }
    }
}