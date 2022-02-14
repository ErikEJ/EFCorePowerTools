using GalaSoft.MvvmLight.Messaging;
using Moq;
using NUnit.Framework;

namespace UnitTests.ViewModels
{
    using EFCorePowerTools.ViewModels;
    using System.Linq;

    [TestFixture]
    public class SchemaInformationViewModelTests
    {
        [Test]
        public void PropertyChanged_Name_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new SchemaInformationViewModel
            {
                Name = "dbo"
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.Name = "dbo";

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_IsSelected_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new SchemaInformationViewModel
            {
                Name = "dbo",
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.SetSelectedCommand.Execute(false);

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_IsSelected_DifferentValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new SchemaInformationViewModel
            {
                Name = "dbo"
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.SetSelectedCommand.Execute(true);

            // Assert
            Assert.IsTrue(propertyChangedInvoked);
        }

        [Test]
        public void PropertyChanged_IsSelected_ObjectSelection([Values(true, false)] bool isSelected)
        {
            // Arrange
            var vm = new SchemaInformationViewModel
            {
                Name = "dbo"
            };
            vm.Objects.Add(CreateTable("Table1"));
            vm.Objects.Add(CreateTable("Table2"));
            vm.Objects.Add(CreateTable("Table3"));

            // Act
            vm.SetSelectedCommand.Execute(isSelected);

            // Assert
            Assert.IsTrue(vm.Objects.All(c => c.IsSelected == isSelected));
        }

        [Test]
        public void PropertyChanged_IsSelected_PartialObjectSelection()
        {
            // Arrange
            var vm = new SchemaInformationViewModel
            {
                Name = "dbo"
            };
            vm.Objects.Add(CreateTable("Table1"));
            vm.Objects.Add(CreateTable("Table2"));
            vm.Objects.Add(CreateTable("Table3"));

            // Act
            vm.Objects[0].SetSelectedCommand.Execute(true);

            // Assert
            Assert.IsNull(vm.IsSelected);
        }

        private TableInformationViewModel CreateTable(string name)
        {
            var messenger = new Mock<IMessenger>();
            messenger.SetupAllProperties();
            var result = new TableInformationViewModel(messenger.Object);
            result.Name = name;
            return result;
        }
    }
}