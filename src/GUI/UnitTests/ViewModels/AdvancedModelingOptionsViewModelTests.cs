using EFCorePowerTools.Common.Models;
using EFCorePowerTools.Contracts.ViewModels;
using EFCorePowerTools.ViewModels;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace UnitTests.ViewModels
{
    [TestFixture]
    public class AdvancedModelingOptionsViewModelTests
    {
        [Test]
        public void ApplyPresetsPreservesFileFormatWithoutMutatingOriginal()
        {
            IAdvancedModelingOptionsViewModel vm = new AdvancedModelingOptionsViewModel();
            var presets = new ModelingOptionsModel { FileLineEndingStyle = "lf", FileEncoding = "utf-8" };
            vm.ApplyPresets(presets);
            Assert.That(vm.Model.FileLineEndingStyle, Is.EqualTo("lf"));
            Assert.That(vm.Model.FileEncoding, Is.EqualTo("utf-8"));
            vm.Model.FileEncoding = "utf-8-bom";
            Assert.That(presets.FileEncoding, Is.EqualTo("utf-8"));
        }

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