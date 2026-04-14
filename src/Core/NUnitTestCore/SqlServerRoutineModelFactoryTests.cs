using NUnit.Framework;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Routines;
using RevEng.Core.Routines.Procedures;

namespace UnitTests
{
    [TestFixture]
    public class SqlServerRoutineModelFactoryTests
    {
        [Test]
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

            Assert.That(useLegacy, Is.True);
        }

        [Test]
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

            Assert.That(useLegacy, Is.True);
        }

        [Test]
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

            Assert.That(useLegacy, Is.False);
        }

        [Test]
        public void UseStoredProcedureResultSetFallbackDefaultsToTrue()
        {
            var options = new ModuleModelFactoryOptions();

            Assert.That(options.UseStoredProcedureResultSetFallback, Is.True);
        }

        [TestCase(208, "Invalid object name '#OrderTable'.", true)]
        [TestCase(208, "Invalid object name '#OrderLegacyTable'.", true)]
        [TestCase(208, "Invalid object name '#OrderSummaryTable'.", true)]
        [TestCase(208, "Invalid object name '#OrderSearchTable'.", true)]
        public void ShouldTryDefinitionFallbackMatchesObservedDockerPlaygroundLiveDbFailures(int errorNumber, string message, bool expected)
        {
            var result = SqlServerStoredProcedureModelFactory.ShouldTryDefinitionFallback(errorNumber, message);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
