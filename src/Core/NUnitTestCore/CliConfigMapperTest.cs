using AutoFixture.NUnit4;
using NUnit.Framework;
using RevEng.Common;
using RevEng.Common.Cli;
using RevEng.Common.Cli.Configuration;

namespace UnitTests
{
    [TestFixture]
    public class CliConfigMapperTest
    {
        [Test, AutoData]
        public void CanGetOptions(CliConfig config)
        {
            var options = CliConfigMapper.ToOptions(config, "C:\temp", "C:\tempo");

            Assert.That(options.CodeGenerationMode, Is.EqualTo(CodeGenerationMode.EFCore8));
            Assert.That(options.ConnectionString, Is.Null);
            Assert.That(options.ContextClassName, Is.EqualTo(config.Names.DbContextName));
            Assert.That(options.ContextNamespace, Is.EqualTo(config.Names.DbContextNamespace));
            Assert.That(options.DatabaseType, Is.EqualTo(DatabaseType.Undefined));
            Assert.That(options.Dacpac, Is.Null);
            Assert.That(options.DefaultDacpacSchema, Is.Null);
            Assert.That(options.FilterSchemas, Is.False);
            Assert.That(options.IncludeConnectionString, Is.EqualTo(config.CodeGeneration.EnableOnConfiguring));
            Assert.That(options.InstallNuGetPackage, Is.False);
            Assert.That(options.ModelNamespace, Is.EqualTo(config.Names.ModelNamespace));
            Assert.That(options.OptionsPath, Is.Null);
            Assert.That(options.OutputContextPath, Is.EqualTo(config.FileLayout.OutputDbContextPath));
            Assert.That(options.OutputPath, Is.EqualTo(config.FileLayout.OutputPath));
            Assert.That(options.ProjectPath, Is.EqualTo("C:\temp"));
            Assert.That(options.ProjectRootNamespace, Is.EqualTo(config.Names.RootNamespace));
            Assert.That(options.PreserveCasingWithRegex, Is.EqualTo(config.Replacements.PreserveCasingWithRegex));
            Assert.That(options.SelectedHandlebarsLanguage, Is.EqualTo(0));
            Assert.That(options.SelectedToBeGenerated, Is.EqualTo(0));
            Assert.That(options.Tables.Count, Is.GreaterThanOrEqualTo(config.Tables.Count));
            Assert.That(options.UncountableWords, Is.Not.Empty);
            Assert.That(options.PluralRules, Is.Not.Empty);
            Assert.That(options.SingularRules, Is.Not.Empty);
            Assert.That(options.IrregularWords, Is.Not.Empty);
            Assert.That(options.UseBoolPropertiesWithoutDefaultSql, Is.EqualTo(config.CodeGeneration.RemoveDefaultSqlFromBoolProperties));
            Assert.That(options.UseDatabaseNames, Is.EqualTo(config.CodeGeneration.UseDatabaseNames));
            Assert.That(options.UseDatabaseNamesForRoutines, Is.EqualTo(config.CodeGeneration.UseDatabaseNamesForRoutines));
            Assert.That(options.UseDateOnlyTimeOnly, Is.EqualTo(config.TypeMappings.UseDateOnlyTimeOnly));
            Assert.That(options.UseDbContextSplitting, Is.EqualTo(config.FileLayout.SplitDbContextPreview));
            Assert.That(options.UseFluentApiOnly, Is.EqualTo(!config.CodeGeneration.UseDataAnnotations));
            Assert.That(options.UseHandleBars, Is.False);
            Assert.That(options.UseHierarchyId, Is.EqualTo(config.TypeMappings.UseHierarchyId));
            Assert.That(options.UseInflector, Is.EqualTo(config.CodeGeneration.UseInflector));
            Assert.That(options.UseLegacyPluralizer, Is.EqualTo(config.CodeGeneration.UseLegacyInflector));
            Assert.That(options.UseManyToManyEntity, Is.EqualTo(config.CodeGeneration.UseManyToManyEntity));
            Assert.That(options.UseNodaTime, Is.EqualTo(config.TypeMappings.UseNodaTime));
            Assert.That(options.UseNoDefaultConstructor, Is.False);
            Assert.That(options.UseNoNavigations, Is.EqualTo(config.CodeGeneration.UseNoNavigationsPreview));
            Assert.That(options.UseNoObjectFilter, Is.False);
            Assert.That(options.UseNullableReferences, Is.EqualTo(config.CodeGeneration.UseNullableReferenceTypes));
            Assert.That(options.UseSchemaFolders, Is.EqualTo(config.FileLayout.UseSchemaFoldersPreview));
            Assert.That(options.UseSchemaNamespaces, Is.EqualTo(config.FileLayout.UseSchemaNamespacesPreview));
            Assert.That(options.UseSpatial, Is.EqualTo(config.TypeMappings.UseSpatial));
            Assert.That(options.UseT4, Is.EqualTo(config.CodeGeneration.UseT4));
            Assert.That(options.UseT4Split, Is.EqualTo(config.CodeGeneration.UseT4Split));
            Assert.That(options.UseTypedTvpParameters, Is.EqualTo(config.CodeGeneration.UseTypedTvpParameters));

            Assert.That(options.GetType().GetProperties().Length, Is.EqualTo(55));

            Assert.That(config.GetType().GetProperties().Length, Is.EqualTo(10));
            Assert.That(config.Names.GetType().GetProperties().Length, Is.EqualTo(4));
            Assert.That(config.CodeGeneration.GetType().GetProperties().Length, Is.EqualTo(24));
            Assert.That(config.FileLayout.GetType().GetProperties().Length, Is.EqualTo(5));
            Assert.That(config.Replacements.GetType().GetProperties().Length, Is.EqualTo(5));
            Assert.That(config.TypeMappings.GetType().GetProperties().Length, Is.EqualTo(4));
        }
    }
}
