using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ErikJ.EntityFrameworkCore.Edmx.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding;
using NUnit.Framework;

namespace NUnitTestCore
{
    [TestFixture]
    public class EdmxTest
    {
        private string edmx;

        [SetUp]
        public void Setup()
        {
            edmx = TestPath("AdventureWorks2019.edmx");
        }

        [Test]
        public void Issue551()
        {
            // Arrange
            var factory = new EdmxDatabaseModelFactory(null);
            var options = new DatabaseModelFactoryOptions(null, new List<string>());

            // Act
            var dbModel = factory.Create(TestPath("AdventureWorks2019.edmx"), options);

            // Assert
            Assert.AreEqual(1, dbModel.Tables.Count());
        }

        private string TestPath(string file)
        {
            return Path.Combine(TestContext.CurrentContext.TestDirectory, file);
        }
    }
}
