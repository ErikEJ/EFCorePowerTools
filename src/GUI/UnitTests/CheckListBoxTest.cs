using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErikEJ.SqlCeToolbox.Helpers;
using Xceed.Wpf.Toolkit;
using System.Threading;

namespace UnitTests
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class CheckListBoxTest
    {
        [Test]
        public void CheckboxSelectionUpdatesSourceList()
        {
            // Arrange
            var source = new List<CheckListItem>
            {
                new CheckListItem { IsChecked= true, Label = "Item1" },
                new CheckListItem { IsChecked= true, Label = "Item2" },
                new CheckListItem { IsChecked= true, Label = "Item3" },
                new CheckListItem { IsChecked= true, Label = "Item4" },
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
            Assert.That(source.SingleOrDefault(x => !x.IsChecked).Label, Is.EqualTo("Item1")); // check if the first item is the only item that has property IsChecked as false
        }
    }
}
