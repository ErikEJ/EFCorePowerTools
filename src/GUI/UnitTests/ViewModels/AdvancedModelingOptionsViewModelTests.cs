using EFCorePowerTools.ViewModels;
using NUnit.Framework;
using NUnit.Framework.Legacy;

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
            ClassicAssert.IsNotNull(vm.OkCommand);
            ClassicAssert.IsNotNull(vm.CancelCommand);
        }
    }
}
