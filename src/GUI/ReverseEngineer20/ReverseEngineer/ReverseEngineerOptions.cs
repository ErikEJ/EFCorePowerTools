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
        public OutputPath OutputPath { get; set; } = new OutputPath();
        public string ProjectRootNamespace { get; set; }
        public bool UseFluentApiOnly { get; set; }
        public string ContextClassName { get; set; }
        public List<TableInformationModel> Tables { get; set; }
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

        public static ReverseEngineerOptions FromV2(ReverseEngineerOptionsV2 v2)
        {
            if (v2 == null)
                throw new ArgumentNullException(nameof(v2));

            return new ReverseEngineerOptions
            {
                DatabaseType = v2.DatabaseType,
                ConnectionString = v2.ConnectionString,
                ProjectPath = v2.ProjectPath,
                OutputPath = new OutputPath()
                {
                    Models = v2.OutputPath,
                    Context = v2.OutputPath,
                },
                ProjectRootNamespace = v2.ProjectRootNamespace,
                UseFluentApiOnly = v2.UseFluentApiOnly,
                ContextClassName = v2.ContextClassName,
                Tables = v2.Tables,
                UseDatabaseNames = v2.UseDatabaseNames,
                UseInflector = v2.UseInflector,
                IdReplace = v2.IdReplace,
                UseHandleBars = v2.UseHandleBars,
                IncludeConnectionString = v2.IncludeConnectionString,
                SelectedToBeGenerated = v2.SelectedToBeGenerated,
                Dacpac = v2.Dacpac,
                CustomReplacers = v2.CustomReplacers,
                DefaultDacpacSchema = v2.DefaultDacpacSchema
            };
        }

        public static ReverseEngineerOptions FromV1(ReverseEngineerOptionsV1 v1)
        {
            if (v1 == null)
                throw new ArgumentNullException(nameof(v1));

            return new ReverseEngineerOptions
            {
                DatabaseType = v1.DatabaseType,
                ConnectionString = v1.ConnectionString,
                ProjectPath = v1.ProjectPath,
                OutputPath = new OutputPath()
                {
                    Models = v1.OutputPath,
                    Context = v1.OutputPath,
                },
                ProjectRootNamespace = v1.ProjectRootNamespace,
                UseFluentApiOnly = v1.UseFluentApiOnly,
                ContextClassName = v1.ContextClassName,
                Tables = v1.Tables
                           .Select(m => new TableInformationModel(m, true))
                           .ToList(),
                UseDatabaseNames = v1.UseDatabaseNames,
                UseInflector = v1.UseInflector,
                IdReplace = v1.IdReplace,
                UseHandleBars = v1.UseHandleBars,
                IncludeConnectionString = v1.IncludeConnectionString,
                SelectedToBeGenerated = v1.SelectedToBeGenerated,
                Dacpac = v1.Dacpac,
                CustomReplacers = v1.CustomReplacers,
                DefaultDacpacSchema = v1.DefaultDacpacSchema
            };
        }
    }

    public class ReverseEngineerOptionsV2
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
        public List<TableInformationModel> Tables { get; set; }
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