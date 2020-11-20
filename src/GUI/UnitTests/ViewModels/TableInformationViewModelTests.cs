using NUnit.Framework;

namespace UnitTests.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using EFCorePowerTools.Contracts.ViewModels;
    using EFCorePowerTools.Shared.Models;
    using EFCorePowerTools.ViewModels;
    using RevEng.Shared;
    
    [TestFixture]
    public class TableInformationViewModelTests
    {
        [Test]
        public void PropertyChanged_Name_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new TableInformationViewModel
            {
                Name = "dbo.album",
                ObjectType = ObjectType.Table,
                HasPrimaryKey = true,
                IsSelected = true
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.Name = "dbo.album";

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_IsSelected_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new TableInformationViewModel
            {
                Name = "dbo.album",
                ObjectType = ObjectType.Table,
                HasPrimaryKey = true,
                IsSelected = true
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.IsSelected = true;

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_ObjectType_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new TableInformationViewModel
            {
                Name = "dbo.album",
                ObjectType = ObjectType.Table,
                HasPrimaryKey = true,
                IsSelected = true
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.ObjectType = ObjectType.Table;

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_HasPrimaryKey_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new TableInformationViewModel
            {
                Name = "dbo.album",
                ObjectType = ObjectType.Table,
                HasPrimaryKey = true,
                IsSelected = true
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.HasPrimaryKey = true;

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_Name_DifferentValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new TableInformationViewModel
            {
                Name = "dbo.album",
                ObjectType = ObjectType.Table,
                HasPrimaryKey = true,
                IsSelected = true
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.Name = "dbo.album2";

            // Assert
            Assert.IsTrue(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_IsSelected_DifferentValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new TableInformationViewModel
            {
                Name = "dbo.album",
                ObjectType = ObjectType.Table,
                HasPrimaryKey = true,
                IsSelected = true
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.IsSelected = false;

            // Assert
            Assert.IsTrue(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_ObjectType_DifferentValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new TableInformationViewModel
            {
                Name = "dbo.album",
                ObjectType = ObjectType.Table,
                HasPrimaryKey = true,
                IsSelected = true
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.ObjectType = ObjectType.ScalarFunction;

            // Assert
            Assert.IsTrue(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_HasPrimaryKey_DifferentValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new TableInformationViewModel
            {
                Name = "dbo.album",
                ObjectType = ObjectType.Table,
                HasPrimaryKey = true,
                IsSelected = true
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.HasPrimaryKey = false;

            // Assert
            Assert.IsTrue(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_IsSelected_ColumnsSelection([Values(true, false)] bool isSelected)
        {
            // Arrange
            var vm = new TableInformationViewModel
            {
                Name = "dbo.album",
                ObjectType = ObjectType.Table,
                HasPrimaryKey = true,
                IsSelected = isSelected,
            };
            vm.Columns.Add(new ColumnInformationViewModel { Name = "column1" });
            vm.Columns.Add(new ColumnInformationViewModel { Name = "column2" });
            vm.Columns.Add(new ColumnInformationViewModel { Name = "column3" });


            // Act
            vm.IsSelected = !isSelected;

            // Assert
            Assert.IsTrue(vm.Columns.All(c => c.IsSelected == !isSelected));
        }
    }
}