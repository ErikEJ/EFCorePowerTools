using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using RevEng.Common;
//using NUnit.Framework.Legacy;
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

            options.CodeGenerationMode.Should().Be(CodeGenerationMode.EFCore8);
            options.ConnectionString.Should().Be(null);
            options.ContextClassName.Should().Be(config.Names.DbContextName);
            options.ContextNamespace.Should().Be(config.Names.DbContextNamespace);
            options.DatabaseType.Should().Be(DatabaseType.Undefined);
            options.Dacpac.Should().Be(null);
            options.DefaultDacpacSchema.Should().Be(null);
            options.FilterSchemas.Should().Be(false);
            options.IncludeConnectionString.Should().Be(config.CodeGeneration.EnableOnConfiguring);
            options.InstallNuGetPackage.Should().Be(false);
            options.ModelNamespace.Should().Be(config.Names.ModelNamespace);
            options.OptionsPath.Should().Be(null);
            options.OutputContextPath.Should().Be(config.FileLayout.OutputDbContextPath);
            options.OutputPath.Should().Be(config.FileLayout.OutputPath);
            options.ProjectPath.Should().Be("C:\temp");
            options.ProjectRootNamespace.Should().Be(config.Names.RootNamespace);
            options.PreserveCasingWithRegex.Should().Be(config.Replacements.PreserveCasingWithRegex);
            options.SelectedHandlebarsLanguage.Should().Be(0);
            options.SelectedToBeGenerated.Should().Be(0);
            options.Tables.Count.Should().BeGreaterThanOrEqualTo(config.Tables.Count);
            options.UncountableWords.Should().NotBeEmpty();
            options.PluralRules.Should().NotBeEmpty();
            options.SingularRules.Should().NotBeEmpty();
            options.IrregularWords.Should().NotBeEmpty();
            options.UseBoolPropertiesWithoutDefaultSql.Should().Be(config.CodeGeneration.RemoveDefaultSqlFromBoolProperties);
            options.UseDatabaseNames.Should().Be(config.CodeGeneration.UseDatabaseNames);
            options.UseDatabaseNamesForRoutines.Should().Be(config.CodeGeneration.UseDatabaseNamesForRoutines);
            options.UseDateOnlyTimeOnly.Should().Be(config.TypeMappings.UseDateOnlyTimeOnly);
            options.UseDbContextSplitting.Should().Be(config.FileLayout.SplitDbContextPreview);
            options.UseFluentApiOnly.Should().Be(!config.CodeGeneration.UseDataAnnotations);
            options.UseHandleBars.Should().Be(false);
            options.UseHierarchyId.Should().Be(config.TypeMappings.UseHierarchyId);
            options.UseInflector.Should().Be(config.CodeGeneration.UseInflector);
            options.UseLegacyPluralizer.Should().Be(config.CodeGeneration.UseLegacyInflector);
            options.UseManyToManyEntity.Should().Be(config.CodeGeneration.UseManyToManyEntity);
            options.UseNodaTime.Should().Be(config.TypeMappings.UseNodaTime);
            options.UseNoDefaultConstructor.Should().Be(false);
            options.UseNoNavigations.Should().Be(config.CodeGeneration.UseNoNavigationsPreview);
            options.UseNoObjectFilter.Should().Be(false);
            options.UseNullableReferences.Should().Be(config.CodeGeneration.UseNullableReferenceTypes);
            options.UseSchemaFolders.Should().Be(config.FileLayout.UseSchemaFoldersPreview);
            options.UseSchemaNamespaces.Should().Be(config.FileLayout.UseSchemaNamespacesPreview);
            options.UseSpatial.Should().Be(config.TypeMappings.UseSpatial);
            options.UseT4.Should().Be(config.CodeGeneration.UseT4);
            options.UseT4Split.Should().Be(config.CodeGeneration.UseT4Split);
            options.UseTypedTvpParameters.Should().Be(config.CodeGeneration.UseTypedTvpParameters);

            options.GetType().GetProperties().Length.Should().Be(55);

            config.GetType().GetProperties().Length.Should().Be(10);
            config.Names.GetType().GetProperties().Length.Should().Be(4);
            config.CodeGeneration.GetType().GetProperties().Length.Should().Be(24);
            config.FileLayout.GetType().GetProperties().Length.Should().Be(5);
            config.Replacements.GetType().GetProperties().Length.Should().Be(5);
            config.TypeMappings.GetType().GetProperties().Length.Should().Be(4);
        }
    }
}
