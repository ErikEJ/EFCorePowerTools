using NUnit.Framework;

namespace UnitTests.ViewModels
{
    using System.Collections.Generic;
    using EFCorePowerTools.Shared.Models;
    using EFCorePowerTools.ViewModels;

    [TestFixture]
    public class TableInformationViewModelTests
    {
        [Test]
        public void PropertyChanged_Model_SaveValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var tim = new TableInformationModel("dbo.Album", true);
            var vm = new TableInformationViewModel
            {
                Model = tim,
                IsSelected = true
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.Model = tim;

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_IsSelected_SaveValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var tim = new TableInformationModel("dbo.Album", true);
            var vm = new TableInformationViewModel
            {
                Model = tim,
                IsSelected = true
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.IsSelected = true;

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_Model_DifferentValue()
        {
            // Arrange
            var changedProperties = new List<string>();
            var tim = new TableInformationModel("dbo.Album", true);
            var vm = new TableInformationViewModel
            {
                Model = tim,
                IsSelected = true
            };
            vm.PropertyChanged += (sender, args) => changedProperties.Add(args.PropertyName);

            // Act
            vm.Model = null;

            // Assert
            Assert.AreEqual(1, changedProperties.Count);
            Assert.AreEqual(nameof(TableInformationViewModel.Model), changedProperties[0]);
        }

        [Test]
        public void PropertyChanged_IsSelected_DifferentValue()
        {
            // Arrange
            var changedProperties = new List<string>();
            var tim = new TableInformationModel("dbo.Album", true);
            var vm = new TableInformationViewModel
            {
                Model = tim,
                IsSelected = true
            };
            vm.PropertyChanged += (sender, args) => changedProperties.Add(args.PropertyName);

            // Act
            vm.IsSelected = false;

            // Assert
            Assert.AreEqual(1, changedProperties.Count);
            Assert.AreEqual(nameof(TableInformationViewModel.IsSelected), changedProperties[0]);
        }
    }
}