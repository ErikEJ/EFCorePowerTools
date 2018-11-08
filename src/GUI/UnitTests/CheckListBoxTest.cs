using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using ErikEJ.SqlCeToolbox.Helpers;
using Xceed.Wpf.Toolkit;
using System.Threading;

namespace UnitTests
{
    using EFCorePowerTools.Shared.Models;

    [TestFixture, Apartment(ApartmentState.STA)]
    public class CheckListBoxTest
    {
        [Test]
        public void CheckboxSelectionUpdatesSourceList()
        {
            // Arrange
            var source = new List<CheckListItem>
            {
                new CheckListItem { IsChecked= true, TableInformationModel = new TableInformationModel("dbo", "Item1", true) },
                new CheckListItem { IsChecked= true, TableInformationModel = new TableInformationModel("dbo", "Item2", true) },
                new CheckListItem { IsChecked= true, TableInformationModel = new TableInformationModel("dbo", "Item3", true) },
                new CheckListItem { IsChecked= true, TableInformationModel = new TableInformationModel("dbo", "Item4", true) },
            };

            var checkListBox = new CheckListBox
            {
                ItemsSource = source,
                SelectedMemberPath = "IsChecked"
            };

            // Act
            ((CheckListItem)checkListBox.Items[0]).IsChecked = false;

            // Assert
            Assert.That(source[0].IsChecked, Is.False); // check if the first item has property IsChecked as false
            Assert.That(source.SingleOrDefault(x => !x.IsChecked).TableInformationModel.UnsafeFullName, Is.EqualTo("dbo.Item1")); // check if the first item is the only item that has property IsChecked as false
        }
    }
}
