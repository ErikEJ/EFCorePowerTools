using System.Linq;
using NUnit.Framework;
using RevEng.Common.Cli;
using RevEng.Common.Cli.Configuration;

namespace UnitTests
{
    [TestFixture]
    public class CliObjectListTest
    {
        [Test]
        public void CanGetConfig()
        {
            var config = GetConfig();

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);

            Assert.AreEqual(5, result.Count);
        }

        [Test]
        public void CanUseGobalFilter()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);

            Assert.IsEmpty(result);
        }

        [Test]
        public void CanAddStartsWithFilter()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "[dbo]*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);

            Assert.AreEqual(1, result.Count);
        }

        [Test]
        public void CanAddStartsWithFilter_2()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "[other]*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);

            Assert.AreEqual(4, result.Count);
        }

        [Test]
        public void CanAddStartsWithFilterAndExplicitInclude()
        {
            var config = GetConfig();

            config.Tables.First().Exclude = false;

            config.Tables.Add(new Table { ExclusionWildcard = "[dbo]*" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);

            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public void CanAddEndsWithFilter()
        {
            var config = GetConfig();

            config.Tables.Add(new Table { ExclusionWildcard = "*ors]" });

            var result = CliConfigMapper.BuildObjectList(config);

            Assert.NotNull(result);

            Assert.AreEqual(3, result.Count);
        }

        private CliConfig GetConfig()
        {
            var config = new CliConfig
            {
                Tables = new System.Collections.Generic.List<Table>
                {
                    new Table
                    {
                        Name = "[dbo].[Users]",
                    },
                    new Table
                    {
                        Name = "[other].[Accounts]",
                    },
                    new Table
                    {
                        Name = "[dbo].[Extras]",
                    },
                    new Table
                    {
                        Name = "[dbo].[Actors]",
                    },
                    new Table
                    {
                        Name = "[dbo].[Directors]",
                    },
                }
            };

            return config;
        }
    }
}
