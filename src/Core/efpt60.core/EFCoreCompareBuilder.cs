using EfSchemaCompare;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Design.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Modelling
{
    public static class EfCoreCompareBuilder
    {
#pragma warning disable CA1002 // Do not expose generic lists
        public static List<Tuple<string, string>> GenerateDbContextList(string outputPath, string startupOutputPath)
        {
            return BuildResult(outputPath, startupOutputPath ?? outputPath, true);
        }

        public static List<Tuple<string, string>> GenerateSchemaCompareResult(string outputPath, string startupOutputPath, string connectionString, string dbContexts)
        {
            ArgumentNullException.ThrowIfNull(dbContexts);

            return GetCompareResult(outputPath, startupOutputPath ?? outputPath, connectionString, dbContexts);
        }
#pragma warning restore CA1002 // Do not expose generic lists

        private static List<Tuple<string, string>> GetCompareResult(string outputPath, string startupOutputPath, string connectionString, string dbContexts)
        {
            var result = new List<Tuple<string, string>>();

            var dbContextNameList = dbContexts.Split(',').ToHashSet();

            var dbContextList = new List<DbContext>();

            var operations = GetOperations(outputPath, startupOutputPath);
            var types = GetDbContextTypes(operations);

            foreach (var type in types)
            {
                try
                {
                    if (dbContextNameList.Contains(type.Name))
                    {
                        var context = operations.CreateContext(type.Name);
                        dbContextList.Add(context);
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Console.Error.WriteLine(ex);
                }
            }

            var comparer = new CompareEfSql();

            comparer.CompareEfWithDb(connectionString, dbContextList.ToArray());

            var logsJson = Newtonsoft.Json.JsonConvert.SerializeObject(comparer.Logs);

            result.Add(new Tuple<string, string>(dbContextNameList.First(), logsJson));

            return result;
        }

        private static List<Tuple<string, string>> BuildResult(string outputPath, string startupOutputPath,  bool listDbContexts)
        {
            var result = new List<Tuple<string, string>>();
            var operations = GetOperations(outputPath, startupOutputPath);
            var types = GetDbContextTypes(operations);

            if (listDbContexts)
            {
                foreach (var type in types)
                {
                    result.Add(new Tuple<string, string>(type.Name, string.Empty));
                }
            }

            return result;
        }

        private static List<Type> GetDbContextTypes(DbContextOperations operations)
        {
            var types = operations.GetContextTypes().ToList();
            if (types.Count == 0)
            {
                throw new ArgumentException("No EF Core DbContext types found in the project");
            }

            return types;
        }

        private static DbContextOperations GetOperations(string outputPath, string startupOutputPath)
        {
            var assembly = Load(outputPath);
            if (assembly == null)
            {
                throw new ArgumentException("Unable to load project assembly");
            }

            Assembly startupAssembly = null;

            if (startupOutputPath != outputPath)
            {
                startupAssembly = Load(startupOutputPath);
            }

            var reporter = new OperationReporter(new OperationReportHandler());

            return new DbContextOperations(reporter, assembly, startupAssembly ?? assembly, outputPath, null, null, false, Array.Empty<string>());
        }

        private static Assembly Load(string assemblyPath)
        {
            return File.Exists(assemblyPath) ? Assembly.LoadFile(assemblyPath) : null;
        }
    }
}
