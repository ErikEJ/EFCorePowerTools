using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Dgml;
using Xunit;

namespace UnitTests
{
    public class DgmlTest
    {
        private readonly string template;

        public DgmlTest()
        {
            template = GetTemplate();
        }

        [Fact]
        public void ParseDebugViewSample1()
        {
            // Arrange
            var debugView = ReadAllLines("Aw2014Person.txt");

            // Act
            var result = DebugViewParser.Parse(debugView, "Test");

            // Assert
            Assert.Equal(110, result.Nodes.Count);
            Assert.Equal(122, result.Links.Count);
            Assert.Equal(2, result.Links.Count(n => n.Contains("IsUnique=\"True\"")));
        }

        [Fact]
        public void ParseDebugViewFkBug()
        {
            // Arrange
            var debugView = ReadAllLines("Northwind.txt");

            // Act
            var result = DebugViewParser.Parse(debugView, "Test");

            // Assert
            Assert.Equal(129, result.Nodes.Count);
            Assert.Equal(141, result.Links.Count);
            Assert.Equal(0, result.Links.Count(n => n.Contains("IsUnique=\"True\"")));
        }

        [Fact]
        public void ParseDebugViewMultiColFk()
        {
            // Arrange
            var debugView = ReadAllLines("Pfizer.txt");

            // Act
            var result = DebugViewParser.Parse(debugView, "Test");

            // Assert
            Assert.Equal(160, result.Nodes.Count);
            Assert.Equal(172, result.Links.Count);
        }

        [Fact]
        public void ParseDebugViewIssue604()
        {
            // Arrange
            var debugView = ReadAllLines("Issue604.txt");

            // Act
            var result = DebugViewParser.Parse(debugView, "Test");

            // Assert
            Assert.Equal(124, result.Nodes.Count);
            Assert.Equal(150, result.Links.Count);
        }

        [Fact]
        public void BuildChinook()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("ChinookContext.txt"), "test", template);

            // Assert
            Assert.NotNull(result);

            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, @"Chinook.dgml"), result, Encoding.UTF8);
        }

        [Fact]
        public void BuildSample1()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("Aw2014Person.txt"), "test", template);

            // Assert
            Assert.NotNull(result);

            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, @"Aw2014Person.dgml"), result, Encoding.UTF8);
        }

        [Fact]
        public void BuildNorthwind()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("Northwind.txt"), "test", template);

            // Assert
            Assert.NotNull(result);

            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, @"northwind.dgml"), result, Encoding.UTF8);
        }

        [Fact]
        public void BuildPfizer()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("Pfizer.txt"), "test", template);

            // Assert
            Assert.NotNull(result);

            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, @"pfizer.dgml"), result, Encoding.UTF8);
        }

        [Fact]
        public void BuildBNoFk()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("NoFk.txt"), "test", template);

            // Assert
            Assert.NotNull(result);

            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "nofk.dgml"), result, Encoding.UTF8);
        }

        [Fact]
        public void BuildSingleNav()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("SingleNav.txt"), "test", template);

            // Assert
            Assert.NotNull(result);

            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, @"singlenav.dgml"), result, Encoding.UTF8);
        }

        [Fact]
        public void BuildSamurai()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("Samurai.txt"), "test", template);

            // Assert
            Assert.NotNull(result);

            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, @"Samurai.dgml"), result, Encoding.UTF8);
        }

        [Fact]
        public void BuildIssue604()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("Issue604.txt"), "test", template);

            // Assert
            Assert.NotNull(result);

            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, @"Issue604.dgml"), result, Encoding.UTF8);
        }

        [Fact]
        public void BuildIssue687()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("Issue687.txt"), "test", template);

            // Assert
            Assert.NotNull(result);

            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, @"Issue604.dgml"), result, Encoding.UTF8);
        }

        [Fact]
        public void BuildIdentity()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("Identity.txt"), "test", template);

            // Assert
            Assert.NotNull(result);

            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, @"Identity.dgml"), result, Encoding.UTF8);
        }

        [Fact]
        public void BuildLongView50()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("longview50.txt"), "test", template);

            // Assert
            Assert.NotNull(result);

            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, @"LongView50.dgml"), result, Encoding.UTF8);
        }

        private static string GetTemplate()
        {
            var resourceName = "NUnitTestCore.template.dgml";

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private string[] ReadAllLines(string file)
        {
            return File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "Dgml", file));
        }

        private string ReadAllText(string file)
        {
            return File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Dgml", file));
        }
    }
}
