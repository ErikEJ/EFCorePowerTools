using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Dgml;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace UnitTests
{
    [TestFixture]
    public class DgmlTest
    {
        private string template;

        [SetUp]
        public void Setup()
        {
            template = GetTemplate();
        }

        [Test]
        public void ParseDebugViewSample1()
        {
            // Arrange
            var debugView = ReadAllLines("Aw2014Person.txt");

            // Act
            var result = DebugViewParser.Parse(debugView, "Test");

            // Assert
            ClassicAssert.AreEqual(110, result.Nodes.Count);
            ClassicAssert.AreEqual(122, result.Links.Count);

            ClassicAssert.AreEqual(2, result.Links.Count(n => n.Contains("IsUnique=\"True\"")));
        }

        [Test]
        public void ParseDebugViewFkBug()
        {
            // Arrange
            var debugView = ReadAllLines("Northwind.txt");

            // Act
            var result = DebugViewParser.Parse(debugView, "Test");

            // Assert
            ClassicAssert.AreEqual(129, result.Nodes.Count);
            ClassicAssert.AreEqual(141, result.Links.Count);

            ClassicAssert.AreEqual(0, result.Links.Count(n => n.Contains("IsUnique=\"True\"")));
        }

        [Test]
        public void ParseDebugViewMultiColFk()
        {
            // Arrange
            var debugView = ReadAllLines("Pfizer.txt");

            // Act
            var result = DebugViewParser.Parse(debugView, "Test");

            // Assert
            ClassicAssert.AreEqual(160, result.Nodes.Count);
            ClassicAssert.AreEqual(172, result.Links.Count);
        }

        [Test]
        public void ParseDebugViewIssue604()
        {
            // Arrange
            var debugView = ReadAllLines("Issue604.txt");

            // Act
            var result = DebugViewParser.Parse(debugView, "Test");

            // Assert
            ClassicAssert.AreEqual(124, result.Nodes.Count);
            ClassicAssert.AreEqual(150, result.Links.Count);
        }

        [Test]
        public void BuildChinook()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("ChinookContext.txt"), "test", template);

            // Assert
            ClassicAssert.AreNotEqual(null, result);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"Chinook.dgml"), result, Encoding.UTF8);
        }

        [Test]
        public void BuildSample1()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("Aw2014Person.txt"), "test", template);

            // Assert
            ClassicAssert.AreNotEqual(null, result);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"Aw2014Person.dgml"), result, Encoding.UTF8);
        }

        [Test]
        public void BuildNorthwind()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("Northwind.txt"), "test", template);

            // Assert
            ClassicAssert.AreNotEqual(null, result);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"northwind.dgml"), result, Encoding.UTF8);
        }

        [Test]
        public void BuildPfizer()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("Pfizer.txt"), "test", template);

            // Assert
            ClassicAssert.AreNotEqual(null, result);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"pfizer.dgml"), result, Encoding.UTF8);
        }

        [Test]
        public void BuildBNoFk()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("NoFk.txt"), "test", template);

            // Assert
            ClassicAssert.AreNotEqual(null, result);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "nofk.dgml"), result, Encoding.UTF8);
        }

        [Test]
        public void BuildSingleNav()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("SingleNav.txt"), "test", template);

            // Assert
            ClassicAssert.AreNotEqual(null, result);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"singlenav.dgml"), result, Encoding.UTF8);
        }

        [Test]
        public void BuildSamurai()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("Samurai.txt"), "test", template);

            // Assert
            ClassicAssert.AreNotEqual(null, result);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"Samurai.dgml"), result, Encoding.UTF8);
        }

        [Test]
        public void BuildIssue604()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("Issue604.txt"), "test", template);

            // Assert
            ClassicAssert.AreNotEqual(null, result);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"Issue604.dgml"), result, Encoding.UTF8);
        }

        [Test]
        public void BuildIssue687()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("Issue687.txt"), "test", template);

            // Assert
            ClassicAssert.AreNotEqual(null, result);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"Issue604.dgml"), result, Encoding.UTF8);
        }

        [Test]
        public void BuildIdentity()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("Identity.txt"), "test", template);

            // Assert
            ClassicAssert.AreNotEqual(null, result);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"Identity.dgml"), result, Encoding.UTF8);
        }

        [Test]
        public void BuildLongView50()
        {
            // Act
            var result = DgmlBuilder.Build(ReadAllText("longview50.txt"), "test", template);

            // Assert
            ClassicAssert.AreNotEqual(null, result);

            File.WriteAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, @"LongView50.dgml"), result, Encoding.UTF8);
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
            return File.ReadAllLines(Path.Combine(TestContext.CurrentContext.TestDirectory, "Dgml", file));
        }

        private string ReadAllText(string file)
        {
            return File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "Dgml", file));
        }
    }
}
