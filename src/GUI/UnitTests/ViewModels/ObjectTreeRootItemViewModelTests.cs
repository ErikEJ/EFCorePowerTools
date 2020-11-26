using NUnit.Framework;

namespace UnitTests.ViewModels
{
    using EFCorePowerTools.Contracts.ViewModels;
    using EFCorePowerTools.ViewModels;
    using System.Linq;

    [TestFixture]
    public class ObjectTreeRootItemViewModelTests
    {
        [Test]
        public void IsSelected_AllSelected()
        {
            // Arrange
            var vm = new ObjectTreeRootItemViewModel();
            AddObjects(vm);

            // Act
            foreach (var item in vm.Objects)
            {
                item.IsSelected = true;
            }

            // Assert
            Assert.IsTrue(vm.IsSelected);
        }

        [Test]
        public void IsSelected_NoneSelected()
        {
            // Arrange
            var vm = new ObjectTreeRootItemViewModel();
            AddObjects(vm);

            // Act
            foreach (var item in vm.Objects)
            {
                item.IsSelected = false;
            }

            // Assert
            Assert.IsFalse(vm.IsSelected);
        }

        [Test]
        public void IsSelected_PartialSelection()
        {
            // Arrange
            var vm = new ObjectTreeRootItemViewModel();
            AddObjects(vm);

            // Act
            foreach (var item in vm.Objects.Take(3))
            {
                item.IsSelected = vm.Objects.IndexOf(item) % 2 == 0;
            }

            // Assert
            Assert.IsNull(vm.IsSelected);
        }

        [Test]
        public void IsVisible_NoneVisible()
        {
            // Arrange
            var vm = new ObjectTreeRootItemViewModel();
            AddObjects(vm);

            // Act
            foreach (var item in vm.Objects)
            {
                item.IsVisible = false;
            }

            // Assert
            Assert.IsFalse(vm.IsVisible);
        }

        [Test]
        public void IsVisible_PartialVisible()
        {
            // Arrange
            var vm = new ObjectTreeRootItemViewModel();
            AddObjects(vm);

            // Act
            foreach (var item in vm.Objects.Take(1))
            {
                item.IsVisible = false;
            }

            // Assert
            Assert.IsTrue(vm.IsVisible);
        }

        [Test]
        public void Text_SameValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new ObjectTreeRootItemViewModel();
            vm.Text = "Tables";
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.Text = "Tables";

            // Assert
            Assert.IsFalse(propertyChangedInvoked);
        }

        [Test]
        public void Text_DifferentValue()
        {
            // Arrange
            var propertyChangedInvoked = false;
            var vm = new ObjectTreeRootItemViewModel();
            vm.Text = "Tables";
            vm.PropertyChanged += (sender, args) => propertyChangedInvoked = true;

            // Act
            vm.Text = "Views";

            // Assert
            Assert.IsTrue(propertyChangedInvoked);
        }


        private static void AddObjects(ObjectTreeRootItemViewModel vm)
        {
            foreach (var item in GetDatabaseObjects())
            {
                vm.Objects.Add(item);
            }
        }

        private static ITableInformationViewModel[] GetDatabaseObjects()
        {
            var r = new ITableInformationViewModel[6];

            r[0] = new TableInformationViewModel
            {
                IsSelected = false,
                HasPrimaryKey = true,
                Name = "[dbo].[Atlas]",
            };
            r[0].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "Id"
            });
            r[0].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column1"
            });
            r[0].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column2"
            });

            r[1] = new TableInformationViewModel
            {
                IsSelected = true,
                HasPrimaryKey = true,
                Name = "[__].[RefactorLog]",
            };
            r[1].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "Id"
            });
            r[1].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column1"
            });
            r[1].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column2"
            });

            r[2] = new TableInformationViewModel
            {
                IsSelected = true,
                HasPrimaryKey = true,
                Name = "[dbo].[__RefactorLog]",
            };
            r[2].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "Id"
            });
            r[2].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column1"
            });
            r[2].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column2"
            });

            r[3] = new TableInformationViewModel
            {
                IsSelected = true,
                HasPrimaryKey = true,
                Name = "[dbo].[sysdiagrams]",
            };
            r[3].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "Id"
            });
            r[3].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column1"
            });
            r[3].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column2"
            });

            r[4] = new TableInformationViewModel
            {
                IsSelected = true,
                HasPrimaryKey = true,
                Name = "unit.test",
            };
            r[4].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "Id"
            });
            r[4].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column1"
            });
            r[4].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column2"
            });

            r[5] = new TableInformationViewModel
            {
                IsSelected = true,
                HasPrimaryKey = true,
                Name = "unit.foo",
            };
            r[5].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "Id"
            });
            r[5].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column1"
            });
            r[5].Columns.Add(new ColumnInformationViewModel
            {
                IsSelected = true,
                Name = "column2"
            });

            return r;
        }
    }
}