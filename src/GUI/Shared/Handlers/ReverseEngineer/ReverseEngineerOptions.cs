using RevEng.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace EFCorePowerTools.Handlers.ReverseEngineer
{
    public class ReverseEngineerOptions
    {
        [IgnoreDataMember]
        public DatabaseType DatabaseType { get; set; }
        [IgnoreDataMember]
        public string ConnectionString { get; set; }
        [IgnoreDataMember]
        public string ProjectPath { get; set; }
        public string OutputPath { get; set; }
        public string OutputContextPath { get; set; }
        public string ProjectRootNamespace { get; set; }
        public string ModelNamespace { get; set; }
        public string ContextNamespace { get; set; }
        public bool UseFluentApiOnly { get; set; } = true;
        public string ContextClassName { get; set; }
        public List<SerializationTableModel> Tables { get; set; }
        public bool UseDatabaseNames { get; set; }
        public bool UseInflector { get; set; }
        public bool UseHandleBars { get; set; }
        public int SelectedHandlebarsLanguage { get; set; }
        public bool IncludeConnectionString { get; set; }
        public int SelectedToBeGenerated { get; set; }
        [IgnoreDataMember]
        public string Dacpac { get; set; }
        [IgnoreDataMember]
        public List<Schema> CustomReplacers { get; set; }
        public string DefaultDacpacSchema { get; set; }
        public bool UseLegacyPluralizer { get; set; }
        public bool UseSpatial { get; set; }
        public bool UseDbContextSplitting { get; set; }
        public bool UseNodaTime { get; set; }
        public bool FilterSchemas { get; set; }
        public bool UseBoolPropertiesWithoutDefaultSql { get; set; }
        public bool UseNullableReferences { get; set; }
        public bool UseNoConstructor { get; set; }
        public bool UseNoNavigations { get; set; }
        public bool UseNoObjectFilter { get; set; }
        public CodeGenerationMode CodeGenerationMode { get; set; }
        public string UiHint { get; set; }
        public List<SchemaInfo> Schemas { get; set; }

        [IgnoreDataMember]
        public bool InstallNuGetPackage { get; set; }
        public bool ProceduresReturnList { get; set; }

        public static ReverseEngineerOptions FromV1(ReverseEngineerOptionsV1 v1)
        {
            if (v1 == null)
                throw new ArgumentNullException(nameof(v1));

            return new ReverseEngineerOptions
            {
                DatabaseType = v1.DatabaseType,
                ConnectionString = v1.ConnectionString,
                ProjectPath = v1.ProjectPath,
                OutputPath = v1.OutputPath,
                ProjectRootNamespace = v1.ProjectRootNamespace,
                UseFluentApiOnly = v1.UseFluentApiOnly,
                ContextClassName = v1.ContextClassName,
                Tables = v1.Tables
                           .Select(m => new SerializationTableModel(m, RevEng.Shared.ObjectType.Table, null))
                           .ToList(),
                UseDatabaseNames = v1.UseDatabaseNames,
                UseInflector = v1.UseInflector,
                UseHandleBars = v1.UseHandleBars,
                SelectedHandlebarsLanguage = 0,
                IncludeConnectionString = v1.IncludeConnectionString,
                SelectedToBeGenerated = v1.SelectedToBeGenerated,
                Dacpac = v1.Dacpac,
                CustomReplacers = v1.CustomReplacers,
                DefaultDacpacSchema = v1.DefaultDacpacSchema
            };
        }
    }

    public class ReverseEngineerOptionsV1
    {
        [IgnoreDataMember]
        public DatabaseType DatabaseType { get; set; }
        [IgnoreDataMember]
        public string ConnectionString { get; set; }
        [IgnoreDataMember]
        public string ProjectPath { get; set; }
        public string OutputPath { get; set; }
        public string ProjectRootNamespace { get; set; }
        public bool UseFluentApiOnly { get; set; }
        public string ContextClassName { get; set; }
        public List<string> Tables { get; set; }
        public bool UseDatabaseNames { get; set; }
        public bool UseInflector { get; set; }
        public bool IdReplace { get; set; }
        public bool UseHandleBars { get; set; }
        public bool IncludeConnectionString { get; set; }
        public int SelectedToBeGenerated { get; set; }
        [IgnoreDataMember]
        public string Dacpac { get; set; }
        [IgnoreDataMember]
        public List<Schema> CustomReplacers { get; set; }
        public string DefaultDacpacSchema { get; set; }
    }
}