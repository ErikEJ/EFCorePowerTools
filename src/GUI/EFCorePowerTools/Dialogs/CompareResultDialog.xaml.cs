namespace EFCorePowerTools.Dialogs
{
    using Contracts.ViewModels;
    using Contracts.Views;
    using EFCorePowerTools.Handlers.Compare;
    using Shared.DAL;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    public partial class CompareResultDialog : ICompareResultDialog
    {
        private readonly Action<IEnumerable<CompareLogModel>> _addComparisonResult;

        public event EventHandler CloseRequested;

        public CompareResultDialog(ITelemetryAccess telemetryAccess,
                                    ICompareResultViewModel viewModel)
        {
            telemetryAccess.TrackPageView(nameof(CompareOptionsDialog));

            DataContext = viewModel;
            viewModel.CloseRequested += (sender, args) =>
            {
                DialogResult = args.DialogResult;
                Close();
            };
            _addComparisonResult = viewModel.AddComparisonResult;
            InitializeComponent();
        }

        public (bool ClosedByOK, object Payload) ShowAndAwaitUserResponse(bool modal)
        {
            bool closedByOkay;

            if (modal)
            {
                closedByOkay = ShowModal() == true;
            }
            else
            {
                closedByOkay = ShowDialog() == true;
            }

            return (closedByOkay, null);
        }

        public void AddComparisonResult(IEnumerable<CompareLogModel> result)
        {
            _addComparisonResult(result);
        }

        //private void GridSplitter_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        //{
        //    e.Handled = SetGridSize();
        //}

        //private void TreeViewItem_Expanded(object sender, RoutedEventArgs e)
        //{
        //    e.Handled = SetGridSize();
        //}

        //private bool SetGridSize()
        //{
        //    var logs = ((ICompareResultViewModel)DataContext).Logs;
        //    var header = FindVisualChild<Grid>(Header, c => c.Name == "GridHeader");
        //    try
        //    {
        //        for (var cindex = 0; cindex < logs.Count; cindex++)
        //        {
        //            var citem = (TreeViewItem)tree.ItemContainerGenerator.ContainerFromIndex(cindex);
        //            var cgrid = FindVisualChild<Grid>(citem, g => g.Name == "Prova");
        //            for (var i = 0; i < cgrid.ColumnDefinitions.Count; i++)
        //            {
        //                cgrid.ColumnDefinitions[i].Width = new GridLength(header.ColumnDefinitions[i].ActualWidth);
        //            }
        //            for (var eindex = 0; eindex < logs[cindex].SubLogs.Count; eindex++)
        //            {
        //                var eitem = (TreeViewItem)citem.ItemContainerGenerator.ContainerFromIndex(eindex);
        //                if (eitem != null)
        //                {
        //                    var egrid = FindVisualChild<Grid>(eitem, g => g.Name == "Prova");
        //                    for (var i = 0; i < egrid.ColumnDefinitions.Count; i++)
        //                    {
        //                        egrid.ColumnDefinitions[i].Width = new GridLength(header.ColumnDefinitions[i].ActualWidth);
        //                    }
        //                    for (var pindex = 0; pindex < logs[cindex].SubLogs[eindex].SubLogs.Count; pindex++)
        //                    {

        //                        var pitem = (TreeViewItem)eitem.ItemContainerGenerator.ContainerFromIndex(pindex);
        //                        if (pitem != null)
        //                        {
        //                            var pgrid = FindVisualChild<Grid>(pitem, g => g.Name == "Prova");
        //                            for (var i = 0; i < pgrid.ColumnDefinitions.Count; i++)
        //                            {
        //                                pgrid.ColumnDefinitions[i].Width = new GridLength(header.ColumnDefinitions[i].ActualWidth);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //private T FindVisualChild<T>(DependencyObject depObj, Func<T, bool> isOk) where T : DependencyObject
        //{
        //    if (depObj != null)
        //    {
        //        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
        //        {
        //            DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
        //            if (child != null && child is T typedChild && isOk(typedChild))
        //            {
        //                return typedChild;
        //            }

        //            T childItem = FindVisualChild<T>(child, isOk);
        //            if (childItem != null) return childItem;
        //        }
        //    }
        //    return null;
        //}

    }
}