using System.Collections.Generic;
using NUnit.Framework;
using RevEng.Core.Abstractions;
using RevEng.Core.Abstractions.Metadata;
using RevEng.Core.Routines;

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
                ModulesUsingLegacyDiscovery = new List<string>
                {
                    "[dbo].[StoGetSomeData]",
                },
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
                ModulesUsingLegacyDiscovery = new List<string>
                {
                    "[dbo].[SomeOtherProcedure]",
                },
            };

            var module = new Procedure
            {
                Schema = "dbo",
                Name = "StoGetSomeData",
            };

            var useLegacy = SqlServerRoutineModelFactory.ShouldUseLegacyResultSetDiscovery(options, module);

            Assert.That(useLegacy, Is.False);
        }
    }
}
