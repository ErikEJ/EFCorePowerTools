using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ErikEJ.SqlCeToolbox.Helpers;
using Microsoft.Win32;
using Xceed.Wpf.Toolkit;

namespace ErikEJ.SqlCeToolbox.Dialogs
{
    public partial class PickTablesDialog
    {
        public PickTablesDialog()
        {
            Telemetry.TrackPageView(nameof(PickTablesDialog));
            InitializeComponent();
            Background = VsThemes.GetWindowBackground();
        }

        public bool IncludeTables { get; set; }

        private List<CheckListItem> items = new List<CheckListItem>();

        public List<string> Tables { get; set; }

        public List<string> SelectedTables { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var table in Tables)
            { 
                var isChecked = !table.StartsWith("__");
                isChecked = !table.StartsWith("dbo.__");
                isChecked = !table.EndsWith(".sysdiagrams");
                items.Add(new CheckListItem { IsChecked = isChecked, Label = table });                
            }
            chkTables.ItemsSource = items;

            //chkTables.ItemSelectionChanged += (src, arg) => {
            //    var test = items;
            //};

            if (SelectedTables != null)
            {
                SetChecked(SelectedTables.ToArray());
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Tables.Clear();
            foreach (var item in items)
            {
                var checkItem = (CheckListItem)item;
                if ((!checkItem.IsChecked && !IncludeTables) 
                    || (checkItem.IsChecked && IncludeTables))
                {
                    Tables.Add(checkItem.Label);
                }
            }
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void chkClear_Click(object sender, RoutedEventArgs e)
        {
            if (chkClear.IsChecked != null && chkClear.IsChecked.Value)
            {
                foreach (var item in items)
                {
                    if (!item.IsChecked)
                    {
                        item.IsChecked = true;
                    }
                }
            }
            else
            {
                foreach (var item in items)
                {
                    if (item.IsChecked)
                    {
                        item.IsChecked = false;
                    }
                }
            }
            chkTables.ItemsSource = null;
            chkTables.ItemsSource = items;

            TxtSearchTable.Text = string.Empty;
        }

        private void BtnSaveSelection_OnClick(object sender, RoutedEventArgs e)
        {
            var tableList = string.Empty;
            foreach (var item in items)
            {
                var checkItem = (CheckListItem)item;
                if ((checkItem.IsChecked))
                {
                    tableList += checkItem.Label + Environment.NewLine;
                }
            }

            var sfd = new SaveFileDialog
            {
                Filter = "Text file (*.txt)|*.txt|All Files(*.*)|*.*",
                ValidateNames = true,
                Title = "Save list of tables as"
            };
            if (sfd.ShowDialog() != true) return;
            File.WriteAllText(sfd.FileName, tableList, Encoding.UTF8);
        }

        private void BtnLoadSelection_OnClick(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Text file (*.txt)|*.txt|All Files(*.*)|*.*",
                CheckFileExists = true,
                Multiselect = false,
                Title = "Select list of tables to load"
            };
            if (ofd.ShowDialog() != true) return;

            var lines = File.ReadAllLines(ofd.FileName);
            SetChecked(lines);
        }

        private void SetChecked(string[] tables)
        {
            foreach (var item in items)
            {
                item.IsChecked = tables.Contains(item.Label);
            }
            chkTables.ItemsSource = null;
            chkTables.ItemsSource = items;

            TxtSearchTable.Text = string.Empty;
        }

        private async void TxtSearchTable_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            var search = TxtSearchTable.Text;

            await Task.Delay(500); // Add a delay (like a debounce) so that not every character change triggers a search
            if (search != TxtSearchTable.Text)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(search))
            {
                chkTables.ItemsSource = items;
                return;
            }

            var filteredItems = items.Where(x => x.Label.ToUpper().Contains(TxtSearchTable.Text.ToUpper())).ToList();
            chkTables.ItemsSource = filteredItems;
        }
    }
}
