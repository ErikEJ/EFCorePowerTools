using System.Collections.Generic;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Routines;
using RevEng.Core.Routines.Procedures;
using Xunit;

namespace UnitTests
{
    public class SqlServerRoutineModelFactoryTests
    {
        [Fact]
        public void ShouldUseLegacyResultSetDiscoveryReturnsTrueWhenGlobalOptionEnabled()
        {
            var options = new ModuleModelFactoryOptions
            {
                UseLegacyResultSetDiscovery = true,
            };

            var module = new Procedure
            {
                Schema = "dbo",
                Name = "StoGetSomeData",
            };

            var useLegacy = SqlServerRoutineModelFactory.ShouldUseLegacyResultSetDiscovery(options, module);

            Assert.True(useLegacy);
        }

        [Fact]
        public void ShouldUseLegacyResultSetDiscoveryReturnsTrueWhenModuleIsConfiguredForLegacyDiscovery()
        {
            var options = new ModuleModelFactoryOptions
            {
                ModulesUsingLegacyDiscovery = ["[dbo].[StoGetSomeData]"],
            };

            var module = new Procedure
            {
                Schema = "dbo",
                Name = "StoGetSomeData",
            };

            var useLegacy = SqlServerRoutineModelFactory.ShouldUseLegacyResultSetDiscovery(options, module);

            Assert.True(useLegacy);
        }

        [Fact]
        public void ShouldUseLegacyResultSetDiscoveryReturnsFalseWhenNeitherGlobalNorModuleOptionIsEnabled()
        {
            var options = new ModuleModelFactoryOptions
            {
                ModulesUsingLegacyDiscovery = ["[dbo].[SomeOtherProcedure]"],
            };

            var module = new Procedure
            {
                Schema = "dbo",
                Name = "StoGetSomeData",
            };

            var useLegacy = SqlServerRoutineModelFactory.ShouldUseLegacyResultSetDiscovery(options, module);

            Assert.False(useLegacy);
        }

        [Fact]
        public void UseStoredProcedureResultSetFallbackDefaultsToTrue()
        {
            var options = new ModuleModelFactoryOptions();

            Assert.True(options.UseStoredProcedureResultSetFallback);
        }

        [Theory]
        [InlineData(208, "Invalid object name '#OrderTable'.", true)]
        [InlineData(208, "Invalid object name '#OrderLegacyTable'.", true)]
        [InlineData(208, "Invalid object name '#OrderSummaryTable'.", true)]
        [InlineData(208, "Invalid object name '#OrderSearchTable'.", true)]
        public void ShouldTryDefinitionFallbackMatchesObservedDockerPlaygroundLiveDbFailures(int errorNumber, string message, bool expected)
        {
            var result = SqlServerStoredProcedureModelFactory.ShouldTryDefinitionFallback(errorNumber, message);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void AddUnnamedColumnErrorsAddsAllUnnamedMessageWhenAllResultColumnsAreUnnamed()
        {
            var errors = new List<string>();
            var module = new Procedure
            {
                Schema = "dbo",
                Name = "StoGetSomeData",
                UnnamedColumnCount = 2,
                Results =
                [
                    [new ModuleResultElement(), new ModuleResultElement()],
                ],
            };

            SqlServerRoutineModelFactory.AddUnnamedColumnErrors(errors, "PROCEDURE", module);

            Assert.Equal(
            [
                "PROCEDURE 'dbo.StoGetSomeData' has 2 unnamed columns.",
                "All result columns are unnamed for PROCEDURE 'dbo.StoGetSomeData'.",
            ], errors);
        }

        [Fact]
        public void AddUnnamedColumnErrorsDoesNotAddAllUnnamedMessageWhenNamedResultColumnsRemain()
        {
            var errors = new List<string>();
            var module = new Procedure
            {
                Schema = "dbo",
                Name = "StoGetSomeData",
                UnnamedColumnCount = 1,
                Results =
                [
                    [new ModuleResultElement(), new ModuleResultElement()],
                ],
            };

            SqlServerRoutineModelFactory.AddUnnamedColumnErrors(errors, "PROCEDURE", module);

            Assert.Equal(
            [
                "PROCEDURE 'dbo.StoGetSomeData' has 1 unnamed columns.",
            ], errors);
        }
    }
}
