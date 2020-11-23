using NUnit.Framework;

namespace UnitTests.ViewModels
{
    using System.Collections.Generic;
    using EFCorePowerTools.Shared.Models;
    using EFCorePowerTools.ViewModels;
    using RevEng.Shared;
    
    [TestFixture]
    public class ColumnInformationViewModelTests
    {
        [Test]
        public void PropertyChanged_Name_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new ColumnInformationViewModel
            {
                Name = "column1",
                IsSelected = true
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.Name = "column1";

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_IsSelected_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new ColumnInformationViewModel
            {
                Name = "column1",
                IsSelected = true
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.IsSelected = true;

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_IsPrimaryKey_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new ColumnInformationViewModel
            {
                Name = "column1",
                IsSelected = true,
                IsPrimaryKey = true
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.IsPrimaryKey = true;

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_Name_DifferentValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new ColumnInformationViewModel
            {
                Name = "column1",
                IsSelected = true
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.Name = "column2";

            // Assert
            Assert.IsTrue(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_IsSelected_DifferentValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new ColumnInformationViewModel
            {
                Name = "column1",
                IsSelected = true
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.IsSelected = false;

            // Assert
            Assert.IsTrue(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_IsPrimaryKey_DifferentValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new ColumnInformationViewModel
            {
                Name = "column1",
                IsSelected = true,
                IsPrimaryKey = true
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.IsPrimaryKey = false;

            // Assert
            Assert.IsTrue(propertyChangedInvoked);
        }
    }
}