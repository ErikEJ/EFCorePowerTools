using ReverseEngineer20.ReverseEngineer;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReverseEngineer20
{
    using System;
    using System.Linq;
    using EFCorePowerTools.Shared.Models;

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
        public bool UseFluentApiOnly { get; set; }
        public string ContextClassName { get; set; }
        public List<TableInformationModel> Tables { get; set; }
        public bool UseDatabaseNames { get; set; }
        public bool UseInflector { get; set; }
        public bool IdReplace { get; set; }
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
        public bool DoNotCombineNamespace { get; set; }
        public bool UseSpatial { get; set; }
        public bool UseDbContextSplitting { get; set; }
        public bool UseNodaTime { get; set; }

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
                           .Select(m => new TableInformationModel(m, true, false))
                           .ToList(),
                UseDatabaseNames = v1.UseDatabaseNames,
                UseInflector = v1.UseInflector,
                IdReplace = v1.IdReplace,
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