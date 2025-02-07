using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using Microsoft.Extensions.DependencyInjection;
using RevEng.Common;
using RevEng.Common.Dab;

namespace RevEng.Core.Diagram
{
    public class ErDiagramBuilder
    {
        private readonly ServiceProvider serviceProvider;
        private readonly DataApiBuilderOptions dataApiBuilderOptions;

        public ErDiagramBuilder(DataApiBuilderOptions dataApiBuilderCommandOptions)
        {
            dataApiBuilderOptions = dataApiBuilderCommandOptions;

            dataApiBuilderOptions.ConnectionString = dataApiBuilderOptions.ConnectionString.ApplyDatabaseType(dataApiBuilderOptions.DatabaseType);

            var options = new ReverseEngineerCommandOptions
            {
                DatabaseType = dataApiBuilderOptions.DatabaseType,
                ConnectionString = dataApiBuilderOptions.ConnectionString,
                Dacpac = dataApiBuilderOptions.Dacpac,
            };

            serviceProvider = new ServiceCollection().AddEfpt(options, [], [], []).BuildServiceProvider();
        }

        public string GetErDiagramFileName()
        {
            var model = GetModelInternal();

            var creator = new DatabaseModelToMermaid(model);

            var diagram = creator.CreateMermaid(createMarkdown: false);

            var fileName = Path.Join(Path.GetTempPath(), Path.GetRandomFileName() + ".mmd");
            File.WriteAllText(fileName, diagram, Encoding.UTF8);

            return fileName;
        }

        private DatabaseModel GetModelInternal()
        {
            var dbModelFactory = serviceProvider.GetService<IDatabaseModelFactory>();

            var dbModelOptions = new DatabaseModelFactoryOptions(dataApiBuilderOptions.Tables.Where(t => t.ObjectType.HasColumns()).Select(m => m.Name), null);

            var dbModel = dbModelFactory!.Create(dataApiBuilderOptions.Dacpac ?? dataApiBuilderOptions.ConnectionString, dbModelOptions);

            return dbModel;
        }
    }
}