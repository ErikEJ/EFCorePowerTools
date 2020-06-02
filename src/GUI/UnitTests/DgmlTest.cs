using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DgmlBuilder;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class DgmlTest
    {
        private readonly DebugViewParser _parser = new DebugViewParser();
        private string _template;

        [SetUp]
        public void Setup()
        {
            _template = GetTemplate();
        }


        [Test]
        public void ParseDebugViewSample1()
        {
            // Arrange
            var debugView = ReadAllLines("Aw2014Person.txt");

            // Act
            var result = _parser.Parse(debugView, "Test");

            // Assert
            Assert.AreEqual(110, result.Nodes.Count);
            Assert.AreEqual(122, result.Links.Count);

            Assert.AreEqual(2, result.Links.Count(n => n.Contains("IsUnique=\"True\"")));
        }

        [Test]
        public void ParseDebugViewFkBug()
        {
            // Arrange
            var debugView = ReadAllLines("Northwind.txt");

            // Act
            var result = _parser.Parse(debugView, "Test");

            // Assert
            Assert.AreEqual(129, result.Nodes.Count);
            Assert.AreEqual(141, result.Links.Count);

            Assert.AreEqual(0, result.Links.Count(n => n.Contains("IsUnique=\"True\"")));
        }

        [Test]
        public void ParseDebugViewMultiColFk()
        {
            // Arrange
            var debugView = ReadAllLines("Pfizer.txt");

            // Act
            var result = _parser.Parse(debugView, "Test");

            // Assert
            Assert.AreEqual(160, result.Nodes.Count);
            Assert.AreEqual(172, result.Links.Count);
        }

        [Test]
        public void ParseDebugViewIssue604()
        {
            // Arrange
            var debugView = ReadAllLines("Issue604.txt");

            // Act
            var result = _parser.Parse(debugView, "Test");

            // Assert
            Assert.AreEqual(124, result.Nodes.Count);
            Assert.AreEqual(150, result.Links.Count);
        }

        [Test]
        public void BuildChinook()
        {
            // Act
            var builder = new DgmlBuilder.DgmlBuilder();
            var result = builder.Build(ReadAllText("ChinookContext.txt"), "test", _template);

            // Assert
            Assert.AreNotEqual(result, null);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"Chinook.dgml"), result, Encoding.UTF8);
        }

        [Test]
        public void BuildSample1()
        {
            // Act
            var builder = new DgmlBuilder.DgmlBuilder();
            var result = builder.Build(ReadAllText("Aw2014Person.txt"), "test", _template);

            // Assert
            Assert.AreNotEqual(result, null);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"Aw2014Person.dgml"), result, Encoding.UTF8);
        }

        [Test]
        public void BuildNorthwind()
        {
            // Act
            var builder = new DgmlBuilder.DgmlBuilder();
            var result = builder.Build(ReadAllText("Northwind.txt"), "test", _template);

            // Assert
            Assert.AreNotEqual(result, null);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"northwind.dgml"), result, Encoding.UTF8);
        }

        [Test]
        public void BuildPfizer()
        {
            // Act
            var builder = new DgmlBuilder.DgmlBuilder();
            var result = builder.Build(ReadAllText("Pfizer.txt"), "test", _template);

            // Assert
            Assert.AreNotEqual(result, null);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"pfizer.dgml"), result, Encoding.UTF8);
        }

        [Test]
        public void BuildBNoFk()
        {
            // Act
            var builder = new DgmlBuilder.DgmlBuilder();
            var result = builder.Build(ReadAllText("NoFk.txt"), "test", _template);

            // Assert
            Assert.AreNotEqual(result, null);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "nofk.dgml"), result, Encoding.UTF8);
        }

        [Test]
        public void BuildSingleNav()
        {
            // Act
            var builder = new DgmlBuilder.DgmlBuilder();
            var result = builder.Build(ReadAllText("SingleNav.txt"), "test", _template);

            // Assert
            Assert.AreNotEqual(result, null);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"singlenav.dgml"), result, Encoding.UTF8);
        }

        [Test]
        public void BuildSamurai()
        {
            // Act
            var builder = new DgmlBuilder.DgmlBuilder();
            var result = builder.Build(ReadAllText("Samurai.txt"), "test", _template);

            // Assert
            Assert.AreNotEqual(result, null);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"Samurai.dgml"), result, Encoding.UTF8);
        }

        [Test]
        public void BuildIssue604()
        {
            // Act
            var builder = new DgmlBuilder.DgmlBuilder();
            var result = builder.Build(ReadAllText("Issue604.txt"), "test", _template);

            // Assert
            Assert.AreNotEqual(result, null);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"Issue604.dgml"), result, Encoding.UTF8);
        }

        [Test]
        public void BuildIdentity()
        {
            // Act
            var builder = new DgmlBuilder.DgmlBuilder();
            var result = builder.Build(ReadAllText("Identity.txt"), "test", _template);

            // Assert
            Assert.AreNotEqual(result, null);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"Identity.dgml"), result, Encoding.UTF8);
        }

        private static string GetTemplate()
        {
            var resourceName = "UnitTests.template.dgml";

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
            return File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, file));
        }

        private string ReadAllText(string file)
        {
            return File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, file));
        }
    }
}
