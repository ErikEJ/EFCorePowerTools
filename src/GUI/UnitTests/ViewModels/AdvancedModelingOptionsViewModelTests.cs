using EFCorePowerTools.ViewModels;
using NUnit.Framework;

namespace UnitTests.ViewModels
{
    [TestFixture]
    public class AdvancedModelingOptionsViewModelTests
    {
        [Test]
        public void Constructor_ArgumentNullException()
        {
            // Arrange & Act
            var vm = new AdvancedModelingOptionsViewModel();

            // Assert
            Assert.IsNotNull(vm.OkCommand);
            Assert.IsNotNull(vm.CancelCommand);
        }
    }
}
