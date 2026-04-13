namespace UnitTests.ViewModels
{
    using EFCorePowerTools.ViewModels;
    using Xunit;
    using RevEng.Common;
    public class PickSchemasViewModelTests
    {
        [Fact]
        public void Constructor_ArgumentNullException()
        {
            // Arrange & Act
            var vm = new PickSchemasViewModel();

            // Assert
            Assert.NotNull(vm.OkCommand);
            Assert.NotNull(vm.CancelCommand);
            Assert.NotNull(vm.AddCommand);
            Assert.NotNull(vm.RemoveCommand);
        }

        [Fact]
        public void Constructor_CollectionInitializedEmpty()
        {
            // Arrange & Act
            var vm = new PickSchemasViewModel();

            // Assert
            Assert.NotNull(vm.Schemas);
            Assert.Empty(vm.Schemas);
        }

        [Fact]
        public void OkCommand_CanNotExecute()
        {
            // Arrange
            var vm = new PickSchemasViewModel();

            // Act
            var canExecute = vm.OkCommand.CanExecute(null);

            // Assert
            Assert.False(canExecute);
        }

        [Fact]
        public void OkCommand_CanExecute()
        {
            // Arrange
            var vm = new PickSchemasViewModel();

            // Act
            vm.Schemas.Add(new SchemaInfo { Name = "TestSchema" });
            var canExecute = vm.OkCommand.CanExecute(null);

            // Assert
            Assert.True(canExecute);
        }

        [Fact]
        public void OkCommand_Executed_EmptySchemasRemoved()
        {
            // Arrange
            var vm = new PickSchemasViewModel();

            // Act
            vm.Schemas.Add(new SchemaInfo { Name = "TestSchema" });
            vm.Schemas.Add(new SchemaInfo { Name = string.Empty });
            vm.Schemas.Add(new SchemaInfo { Name = string.Empty });
            vm.OkCommand.Execute(null);

            // Assert
            Assert.Equal(1, vm.Schemas.Count);
        }

        [Fact]
        public void CancelCommand_CanExecute()
        {
            // Arrange
            var vm = new PickSchemasViewModel();

            // Act
            var canExecute = vm.CancelCommand.CanExecute(null);

            // Assert
            Assert.True(canExecute);
        }

        [Fact]
        public void AddCommand_Executed_SchemaAdded()
        {
            // Arrange
            var vm = new PickSchemasViewModel();

            // Act
            vm.AddCommand.Execute(null);

            // Assert
            Assert.Equal(1, vm.Schemas.Count);
        }

        [Fact]
        public void RemoveCommand_CanNotExecute()
        {
            // Arrange
            var vm = new PickSchemasViewModel();

            // Act
            var canExecute = vm.RemoveCommand.CanExecute(null);

            // Assert
            Assert.False(canExecute);
        }

        [Fact]
        public void RemoveCommand_CanExecute()
        {
            // Arrange
            var vm = new PickSchemasViewModel();
            var testSchema = new SchemaInfo
            {
                Name = "TestSchema",
            };

            // Act
            vm.SelectedSchema = testSchema;
            var canExecute = vm.RemoveCommand.CanExecute(null);

            // Assert
            Assert.True(canExecute);
        }

        [Fact]
        public void RemoveCommand_Executed_SchemaRemoved()
        {
            // Arrange
            var vm = new PickSchemasViewModel();
            var testSchema = new SchemaInfo { Name = "TestSchema" };

            // Act
            vm.Schemas.Add(testSchema);
            vm.SelectedSchema = testSchema;
            vm.RemoveCommand.Execute(null);

            // Assert
            Assert.Equal(0, vm.Schemas.Count);
            Assert.Null(vm.SelectedSchema);
        }
    }
}