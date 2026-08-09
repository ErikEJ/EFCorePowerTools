using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Design.Internal;
using Microsoft.Extensions.DependencyInjection;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Routines.Extensions;
using RevEng.Core.Routines.Procedures;
using Xunit;

namespace UnitTests
{
    [SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Test method names")]
    public class ProcedureScaffolderTests
    {
        [Fact]
        public void ScaffoldModel_WhenHasValidResultSetFalseAndGenerateEmptyResultTypeFalse_ExcludesRoutineFromOutput()
        {
            var scaffolder = CreateScaffolder();
            var model = new RoutineModel
            {
                Routines = new List<Routine> { CreateProcedureWithInvalidResultSet(generateEmptyResultType: false) },
                Errors = new List<string>(),
            };
            var errors = new List<string>();
            var schemas = new List<string>();

            var result = scaffolder.ScaffoldModel(model, CreateScaffolderOptions(), schemas, ref errors);

            Assert.DoesNotContain(result.AdditionalFiles, f => f.Path.Contains("TestProcedureResult"));
            Assert.DoesNotContain("TestProcedure", result.ContextFile.Code);
        }

        [Fact]
        public void ScaffoldModel_WhenHasValidResultSetFalseAndGenerateEmptyResultTypeTrue_IncludesRoutineInOutput()
        {
            var scaffolder = CreateScaffolder();
            var model = new RoutineModel
            {
                Routines = new List<Routine> { CreateProcedureWithInvalidResultSet(generateEmptyResultType: true) },
                Errors = new List<string>(),
            };
            var errors = new List<string>();
            var schemas = new List<string>();

            var result = scaffolder.ScaffoldModel(model, CreateScaffolderOptions(), schemas, ref errors);

            Assert.Contains(result.AdditionalFiles, f => f.Path.Contains("TestProcedureResult"));
            Assert.Contains("TestProcedure", result.ContextFile.Code);
        }

        [Fact]
        public void ScaffoldModel_WhenUdtParameterHasNoSchema_UsesPlainUdtTypeName()
        {
            var scaffolder = CreateScaffolder();
            var model = new RoutineModel
            {
                Routines = new List<Routine> { CreateProcedureWithUdtParameter() },
                Errors = new List<string>(),
            };
            var errors = new List<string>();
            var schemas = new List<string>();

            var result = scaffolder.ScaffoldModel(model, CreateScaffolderOptions(), schemas, ref errors);

            Assert.Contains("UdtTypeName = \"hierarchyid\"", result.ContextFile.Code);
            Assert.DoesNotContain("UdtTypeName = \"[].[hierarchyid]\"", result.ContextFile.Code);
        }

        private static SqlServerStoredProcedureScaffolder CreateScaffolder()
        {
            var services = new ServiceCollection();
            services.AddEntityFrameworkDesignTimeServices();
#pragma warning disable EF1001
            new SqlServerDesignTimeServices().ConfigureDesignTimeServices(services);
#pragma warning restore EF1001
            services.AddSingleton<IClrTypeMapper, SqlServerClrTypeMapper>();
            var provider = services.BuildServiceProvider();
            var code = provider.GetRequiredService<ICSharpHelper>();
            var typeMapper = provider.GetRequiredService<IClrTypeMapper>();
            return new SqlServerStoredProcedureScaffolder(code, typeMapper);
        }

        private static Procedure CreateProcedureWithInvalidResultSet(bool generateEmptyResultType)
        {
            var procedure = new Procedure
            {
                Schema = "dbo",
                Name = "TestProcedure",
                HasValidResultSet = false,
                GenerateEmptyResultType = generateEmptyResultType,
                Results = new List<List<ModuleResultElement>>
                {
                    new List<ModuleResultElement>(),
                },
            };
            procedure.Parameters.Add(new ModuleParameter
            {
                Name = "returnValue",
                StoreType = "int",
                Output = true,
                Nullable = false,
                IsReturnValue = true,
            });
            return procedure;
        }

        private static Procedure CreateProcedureWithUdtParameter()
        {
            var procedure = new Procedure
            {
                Schema = "dbo",
                Name = "TestProcedure",
                HasValidResultSet = true,
                Results = new List<List<ModuleResultElement>>
                {
                    new()
                    {
                        new ModuleResultElement
                        {
                            Name = "ResultValue",
                            StoreType = "int",
                            Nullable = false,
                        },
                    },
                },
            };
            procedure.Parameters.Add(new ModuleParameter
            {
                Name = "parents",
                StoreType = "hierarchyid",
                Output = false,
                Nullable = true,
                TypeName = "hierarchyid",
                TypeSchemaName = null,
            });
            procedure.Parameters.Add(new ModuleParameter
            {
                Name = "returnValue",
                StoreType = "int",
                Output = true,
                Nullable = false,
                IsReturnValue = true,
            });

            return procedure;
        }

        private static ModuleScaffolderOptions CreateScaffolderOptions()
        {
            return new ModuleScaffolderOptions
            {
                ContextName = "TestContext",
                ContextDir = Path.GetTempPath(),
                ContextNamespace = "TestNamespace",
                ModelNamespace = "TestNamespace",
            };
        }
    }
}
