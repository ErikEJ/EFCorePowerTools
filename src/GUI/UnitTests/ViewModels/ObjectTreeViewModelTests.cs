using NUnit.Framework;

namespace UnitTests.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using EFCorePowerTools.Contracts.ViewModels;
    using EFCorePowerTools.Shared.Models;
    using EFCorePowerTools.ViewModels;
    using Moq;
    using RevEng.Shared;

    [TestFixture]
    public class ObjectTreeViewModelTests
    {
        [Test]
        public void Constructors_ArgumentNullException_TableInformationViewModelFactory()
        {
            // Arrange
            Func<ITableInformationViewModel> t = null;
            Func<IColumnInformationViewModel> c = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ObjectTreeViewModel(t, c));
        }

        [Test]
        public void Constructors_ArgumentNullException_ColumnInformationViewModelFactory()
        {
            // Arrange
            Func<ITableInformationViewModel> t = null;
            Func<IColumnInformationViewModel> c = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ObjectTreeViewModel(t, c));
        }

        [Test]
        public void Constructors_CollectionsInitialized()
        {
            // Act
            var vm = new ObjectTreeViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);

            // Assert
            Assert.IsNotNull(vm.Types);
            Assert.IsTrue(vm.Types.Count == 4);
        }

        [Test]
        public void AddObjects_Null()
        {
            // Arrange
            var vm = new ObjectTreeViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => vm.AddObjects(null));
        }

        [Test]
        public void AddObjects_EmptyCollection()
        {
            // Arrange
            var vm = new ObjectTreeViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var objectsToAdd = new TableModel[0];

            // Act
            vm.AddObjects(objectsToAdd);

            // Assert
            Assert.IsEmpty(vm.Types.SelectMany(t => t.Objects));
        }

        [Test]
        public void AddObjects_Collection()
        {
            // Arrange
            var vm = new ObjectTreeViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var objects = GetDatabaseObjects();

            // Act
            vm.AddObjects(objects);
            var vmobjects = vm.Types.SelectMany(t => t.Objects).ToArray();

            // Assert
            Assert.IsNotEmpty(vm.Types.SelectMany(t => t.Objects));
            Assert.AreEqual(objects.Length, vm.Types.SelectMany(t => t.Objects).Count());
            for (var i = 0; i < objects.Count(); i++)
            {
                AreObjectsEqual(objects[i], vmobjects[i]);
                Assert.IsFalse(vm.Types.SelectMany(t => t.Objects).ElementAt(i).IsSelected);
            }
        }

        [Test]
        public void SelectObjects_Null()
        {
            // Arrange
            var vm = new ObjectTreeViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);

            //Act and assert
            Assert.Throws<ArgumentNullException>(() => vm.SelectObjects(null));
        }

        [Test]
        public void SelectTables_EmptyCollection()
        {
            // Arrange
            var vm = new ObjectTreeViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var selectedTables = new SerializationTableModel[0];

            // Act
            vm.SelectObjects(selectedTables);

            // Assert
            Assert.IsEmpty(vm.GetSelectedObjects());
        }

        [Test]
        public void SelectTables_Collection()
        {
            // Arrange
            var vm = new ObjectTreeViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            vm.AddObjects(GetDatabaseObjects());

            // Act
            var selectedObjects = GetSelectedObjects();
            vm.SelectObjects(selectedObjects);

            // Assert
            Assert.AreEqual(GetSelectedObjects().Count(), vm.GetSelectedObjects().Count());
            for (var i = 0; i < vm.GetSelectedObjects().Count(); i++)
            {
                var a = vm.GetSelectedObjects().ElementAt(i);
                var b = selectedObjects.ElementAt(i);

                Assert.AreEqual(a.Name, b.Name);
                Assert.AreEqual(a.ObjectType, b.ObjectType);
                for (var j = 0; j < a.ExcludedColumns.Count(); j++)
                {
                    Assert.AreEqual(a.ExcludedColumns.ElementAt(0), b.ExcludedColumns.ElementAt(0));
                }
            }
        }

        [Test]
        public void SearchText_NoDirectFilter()
        {
            // Arrange
            var vm = new ObjectTreeViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var databaseObjects = GetDatabaseObjects();
            vm.AddObjects(databaseObjects);
            var preFilter = vm.Types.SelectMany(c => c.Objects);

            // Act
            vm.Search("dbo");

            // Assert
            Assert.AreEqual(databaseObjects.Length, preFilter.Count());
            var postFilter = vm.Types.SelectMany(c => c.Objects).Where(c => c.IsVisible);
            Assert.AreEqual(3, postFilter.Count());
        }

        [Test]
        public void SearchText_FilterAfterDelay()
        {
            // Arrange
            var vm = new ObjectTreeViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var databaseObjects = GetDatabaseObjects();
            vm.AddObjects(databaseObjects);

            // Act
            vm.Search("dbo");

            // Assert
            Assert.That(() =>
            {
                var postFilter = vm.Types.SelectMany(c => c.Objects).Where(c => c.IsVisible);
                return postFilter.Count() < databaseObjects.Length && postFilter.All(m => m.Name.Contains("dbo"));
            }, Is.True.After(1500, 200));
        }

        [Test]
        public void GetSelectedObjects_NoTables()
        {
            // Arrange
            var vm = new ObjectTreeViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);

            // Act
            var result = vm.GetSelectedObjects();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetSelectedObjects_WithObjects_NoneSelected()
        {
            // Arrange
            var vm = new ObjectTreeViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            vm.AddObjects(GetDatabaseObjects());

            // Act
            var result = vm.GetSelectedObjects();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetSelectedObjects_WithObjects_AllSelected()
        {
            // Arrange
            var vm = new ObjectTreeViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var databaseObjects = GetDatabaseObjects().ToArray();
            vm.AddObjects(databaseObjects);
            foreach (var item in vm.Types.SelectMany(c => c.Objects))
            {
                item.IsSelected = true;
            }

            // Act
            var result = vm.GetSelectedObjects().ToArray();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Count());
            for (var i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(databaseObjects[i].Name, result[i].Name);
            }
        }

        [Test]
        public void GetSelectedObjects_WithObjects_PartialSelection()
        {
            // Arrange
            var vm = new ObjectTreeViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var databaseObjects = GetDatabaseObjects();
            vm.AddObjects(databaseObjects);
            foreach (var item in vm.Types.Where(c => c.Objects.Any()))
            {
                item.Objects.First().IsSelected = true;
            }

            // Act
            var result = vm.GetSelectedObjects().ToArray();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual(databaseObjects.First(c => c.ObjectType == ObjectType.Table).Name, result[0].Name);
            Assert.AreEqual(databaseObjects.First(c => c.ObjectType == ObjectType.View).Name, result[1].Name);
            Assert.AreEqual(databaseObjects.First(c => c.ObjectType == ObjectType.Procedure).Name, result[2].Name);
        }

        [Test]
        public void GetSelectionState_NoneSelected()
        {
            // Arrange
            var vm = new ObjectTreeViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var databaseObjects = GetDatabaseObjects();
            vm.AddObjects(databaseObjects);

            //Act
            foreach (var item in vm.Types.SelectMany(c => c.Objects))
            {
                item.IsSelected = true;
            }

            //Assert
            Assert.IsTrue(vm.GetSelectionState());
        }

        [Test]
        public void GetSelectionState_AllSelected()
        {
            // Arrange
            var vm = new ObjectTreeViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var databaseObjects = GetDatabaseObjects();
            vm.AddObjects(databaseObjects);

            //Act
            foreach (var item in vm.Types.SelectMany(c => c.Objects))
            {
                item.IsSelected = false;
            }

            //Assert
            Assert.IsFalse(vm.GetSelectionState());
        }

        [Test]
        public void GetSelectionState_PartialSelection()
        {
            // Arrange
            var vm = new ObjectTreeViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var databaseObjects = GetDatabaseObjects();
            vm.AddObjects(databaseObjects);

            //Act
            foreach (var item in vm.Types.SelectMany(c => c.Objects).Take(1))
            {
                item.IsSelected = true;
            }

            //Assert
            Assert.IsNull(vm.GetSelectionState());
        }

        [Test]
        public void SetSelectionState([Values(true, false)] bool selected)
        {
            // Arrange
            var vm = new ObjectTreeViewModel(CreateTableInformationViewModelMockObject, CreateColumnInformationViewModelMockObject);
            var databaseObjects = GetDatabaseObjects();
            vm.AddObjects(databaseObjects);

            //Act
            vm.SetSelectionState(selected);

            //Assert
            if (selected)
                Assert.AreEqual(vm.GetSelectedObjects().Count(), databaseObjects.Length);
            else
                Assert.AreEqual(vm.GetSelectedObjects().Count(), 0);
        }

        private static TableModel[] GetDatabaseObjects()
        {
            IEnumerable<ColumnModel> CreateColumnsWithId()
            {
                return new[]
                {
                    new ColumnModel("Id", true),
                    new ColumnModel("column1", false),
                    new ColumnModel("column2", false)
                };

            }
            var r = new TableModel[10];

            r[0] = new TableModel("[dbo].[Atlas]", true, ObjectType.Table, CreateColumnsWithId());
            r[1] = new TableModel("[__].[RefactorLog]", true, ObjectType.Table, CreateColumnsWithId());
            r[2] = new TableModel("[dbo].[__RefactorLog]", true, ObjectType.Table, CreateColumnsWithId());
            r[3] = new TableModel("[dbo].[sysdiagrams]", true, ObjectType.Table, CreateColumnsWithId());
            r[4] = new TableModel("unit.test", true, ObjectType.Table, CreateColumnsWithId());
            r[5] = new TableModel("unit.foo", true, ObjectType.Table, CreateColumnsWithId());
            r[6] = new TableModel("views.view1", true, ObjectType.View, CreateColumnsWithId());
            r[7] = new TableModel("views.view2", true, ObjectType.View, CreateColumnsWithId());
            r[8] = new TableModel("stored.procedure1", true, ObjectType.Procedure, new ColumnModel[0]);
            r[9] = new TableModel("stored.procedure2", true, ObjectType.Procedure, new ColumnModel[0]);
            return r;
        }

        private static SerializationTableModel[] GetSelectedObjects()
        {
            var r = new SerializationTableModel[5];

            r[0] = new SerializationTableModel("[dbo].[Atlas]", ObjectType.Table, new[] { "column1" });
            r[1] = new SerializationTableModel("unit.test", ObjectType.Table, new string[0]);
            r[2] = new SerializationTableModel("unit.foo", ObjectType.Table, new string[0]);
            r[3] = new SerializationTableModel("views.view1", ObjectType.View, new string[0]);
            r[4] = new SerializationTableModel("stored.procedure2", ObjectType.Procedure, new string[0]);
            return r;
        }

        private static ITableInformationViewModel CreateTableInformationViewModelMockObject()
        {
            var mock = new Mock<ITableInformationViewModel>();
            mock.SetupAllProperties();
            mock.SetupGet(g => g.Columns).Returns(new ObservableCollection<IColumnInformationViewModel>());
            return mock.Object;
        }

        private static IColumnInformationViewModel CreateColumnInformationViewModelMockObject()
        {
            var mock = new Mock<IColumnInformationViewModel>();
            mock.SetupAllProperties();
            return mock.Object;
        }

        private static void AreObjectsEqual(TableModel a, ITableInformationViewModel b)
        {
            Assert.AreEqual(a.HasPrimaryKey, b.HasPrimaryKey);
            Assert.AreEqual(a.Name, b.Name);
            Assert.AreEqual(a.ObjectType, b.ObjectType);
            for (var i = 0; i < a.Columns.Count(); i++)
            {
                Assert.AreEqual(a.Columns.ElementAt(i).Name, b.Columns.ElementAt(i).Name);
            }
        }
    }
}