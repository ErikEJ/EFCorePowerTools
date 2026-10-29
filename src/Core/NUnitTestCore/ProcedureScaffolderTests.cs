using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.EntityFrameworkCore.Design;
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

        [Theory]
        [InlineData("varchar", 25, false, false, 25)]
        [InlineData("nvarchar", 25, false, false, 25)]
        [InlineData("varchar", 25, false, true, 8000)]
        [InlineData("nvarchar", 25, false, true, 4000)]
        [InlineData("varchar", 8000, false, true, 8000)]
        [InlineData("nvarchar", 4000, false, true, 4000)]
        [InlineData("varchar", -1, false, true, -1)]
        [InlineData("nvarchar", -1, false, true, -1)]
        [InlineData("varchar", 25, true, true, 25)]
        [InlineData("nvarchar", 25, true, true, 25)]
        [InlineData("varchar", -1, true, true, -1)]
        [InlineData("nvarchar", -1, true, true, -1)]
        [InlineData("char", 25, false, true, 25)]
        [InlineData("nchar", 25, false, true, 25)]
        [InlineData("varbinary", 25, false, true, 25)]
        [InlineData("binary", 25, false, true, 25)]
        public void ScaffoldModel_UsesExpectedParameterSize(string storeType, int length, bool output, bool useMaxSize, int expectedSize)
        {
            var scaffolder = CreateScaffolder();
            var procedure = CreateProcedureWithInvalidResultSet(generateEmptyResultType: false);
            procedure.HasValidResultSet = true;
            procedure.Parameters.Insert(0, new ModuleParameter
            {
                Name = "Description",
                StoreType = storeType,
                Length = length,
                Output = output,
                Nullable = true,
            });
            var model = new RoutineModel
            {
                Routines = new List<Routine> { procedure },
                Errors = new List<string>(),
            };
            var options = CreateScaffolderOptions();
            options.UseMaxSizeForStringInputParameters = useMaxSize;
            var errors = new List<string>();
            var schemas = new List<string>();

            var result = scaffolder.ScaffoldModel(model, options, schemas, ref errors);

            Assert.Empty(errors);
            Assert.Contains($"Size = {expectedSize},", result.ContextFile.Code);
            Assert.Contains($"SqlDbType = System.Data.SqlDbType.{procedure.Parameters[0].GetSqlDbType()},", result.ContextFile.Code);
            Assert.Contains(output ? "Direction = System.Data.ParameterDirection.InputOutput," : "Value = description ?? Convert.DBNull,", result.ContextFile.Code);

            options.UseMaxSizeForStringInputParameters = false;
            var defaultResult = scaffolder.ScaffoldModel(model, options, schemas, ref errors);
            Assert.Contains($"Size = {length},", defaultResult.ContextFile.Code);
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