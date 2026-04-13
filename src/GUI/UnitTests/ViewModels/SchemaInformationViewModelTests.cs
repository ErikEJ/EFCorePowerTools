using GalaSoft.MvvmLight.Messaging;
using Moq;
using Xunit;

namespace UnitTests.ViewModels
{
    using System.Linq;
    using EFCorePowerTools.ViewModels;

    public class SchemaInformationViewModelTests
    {
        [Fact]
        public void PropertyChanged_Name_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new SchemaInformationViewModel
            {
                Name = "dbo",
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.Name = "dbo";

            // Assert
            Assert.False(propertyChangedInvoked);
        }

        [Fact]
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
            Assert.False(propertyChangedInvoked);
        }

        [Fact]
        public void PropertyChanged_IsSelected_DifferentValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new SchemaInformationViewModel
            {
                Name = "dbo",
            };
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.SetSelectedCommand.Execute(true);

            // Assert
            Assert.True(propertyChangedInvoked);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void PropertyChanged_IsSelected_ObjectSelection(bool isSelected)
        {
            // Arrange
            var vm = new SchemaInformationViewModel
            {
                Name = "dbo",
            };
            vm.Objects.Add(CreateTable("Table1"));
            vm.Objects.Add(CreateTable("Table2"));
            vm.Objects.Add(CreateTable("Table3"));

            // Act
            vm.SetSelectedCommand.Execute(isSelected);

            // Assert
            Assert.True(vm.Objects.All(c => c.IsSelected == isSelected));
        }

        [Fact]
        public void PropertyChanged_IsSelected_PartialObjectSelection()
        {
            // Arrange
            var vm = new SchemaInformationViewModel
            {
                Name = "dbo",
            };
            vm.Objects.Add(CreateTable("Table1"));
            vm.Objects.Add(CreateTable("Table2"));
            vm.Objects.Add(CreateTable("Table3"));

            // Act
            vm.Objects[0].SetSelectedCommand.Execute(true);

            // Assert
            Assert.Null(vm.IsSelected);
        }

        private static TableInformationViewModel CreateTable(string name)
        {
            var messenger = new Mock<IMessenger>();
            messenger.SetupAllProperties();
            var result = new TableInformationViewModel(messenger.Object);
            result.Name = name;
            return result;
        }
    }
}
