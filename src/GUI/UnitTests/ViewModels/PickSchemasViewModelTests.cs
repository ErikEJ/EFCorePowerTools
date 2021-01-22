namespace UnitTests.ViewModels
{
    using EFCorePowerTools.ViewModels;
    using NUnit.Framework;
    using RevEng.Shared;

    [TestFixture]
    public class PickSchemasViewModelTests
    {
        [Test]
        public void Constructor_ArgumentNullException()
        {
            // Arrange & Act
            var vm = new PickSchemasViewModel();

            // Assert
            Assert.IsNotNull(vm.OkCommand);
            Assert.IsNotNull(vm.CancelCommand);
            Assert.IsNotNull(vm.AddCommand);
            Assert.IsNotNull(vm.RemoveCommand);
        }

        [Test]
        public void Constructor_CollectionInitializedEmpty()
        {
            // Arrange & Act
            var vm = new PickSchemasViewModel();

            // Assert
            Assert.IsNotNull(vm.Schemas);
            Assert.IsEmpty(vm.Schemas);
        }

        [Test]
        public void OkCommand_CanNotExecute()
        {
            // Arrange
            var vm = new PickSchemasViewModel();

            // Act
            var canExecute = vm.OkCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [Test]
        public void OkCommand_CanExecute()
        {
            // Arrange
            var vm = new PickSchemasViewModel();

            // Act
            vm.Schemas.Add(new SchemaInfo {Name = "TestSchema"});
            var canExecute = vm.OkCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void OkCommand_Executed_EmptySchemasRemoved()
        {
            // Arrange
            var vm = new PickSchemasViewModel();

            // Act
            vm.Schemas.Add(new SchemaInfo { Name = "TestSchema" });
            vm.Schemas.Add(new SchemaInfo { Name = "" });
            vm.Schemas.Add(new SchemaInfo { Name = "" });
            vm.OkCommand.Execute(null);

            // Assert
            Assert.AreEqual(1, vm.Schemas.Count);
        }

        [Test]
        public void CancelCommand_CanExecute()
        {
            // Arrange
            var vm = new PickSchemasViewModel();

            // Act
            var canExecute = vm.CancelCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
        public void AddCommand_Executed_SchemaAdded()
        {
            // Arrange
            var vm = new PickSchemasViewModel();

            // Act
            vm.AddCommand.Execute(null);

            // Assert
            Assert.AreEqual(1, vm.Schemas.Count);
        }

        [Test]
        public void RemoveCommand_CanNotExecute()
        {
            // Arrange
            var vm = new PickSchemasViewModel();

            // Act
            var canExecute = vm.RemoveCommand.CanExecute(null);

            // Assert
            Assert.IsFalse(canExecute);
        }

        [Test]
        public void RemoveCommand_CanExecute()
        {
            // Arrange
            var vm = new PickSchemasViewModel();
            var testSchema = new SchemaInfo {Name = "TestSchema"};

            // Act
            vm.SelectedSchema = testSchema;
            var canExecute = vm.RemoveCommand.CanExecute(null);

            // Assert
            Assert.IsTrue(canExecute);
        }

        [Test]
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
            Assert.AreEqual(0, vm.Schemas.Count);
            Assert.IsNull(vm.SelectedSchema);
        }
    }
}
