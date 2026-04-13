using EFCorePowerTools.ViewModels;
using Xunit;

namespace UnitTests.ViewModels
{
    public class AdvancedModelingOptionsViewModelTests
    {
        [Fact]
        public void Constructor_ArgumentNullException()
        {
            // Arrange & Act
            var vm = new AdvancedModelingOptionsViewModel();

            // Assert
            Assert.NotNull(vm.OkCommand);
            Assert.NotNull(vm.CancelCommand);
        }
    }
}