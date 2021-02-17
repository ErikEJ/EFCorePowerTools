namespace EFCorePowerTools.ViewModels
{
    using Contracts.EventArgs;
    using EFCorePowerTools.Contracts.ViewModels;
    using EFCorePowerTools.Handlers.Compare;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    public class CompareResultViewModel : ViewModelBase, ICompareResultViewModel
    {
        private bool _showDifferencesOnly = true;
        private List<CompareLogItemViewModel> _completeLogs { get; } = new List<CompareLogItemViewModel>();
        private List<CompareLogItemViewModel> _filteredLogs { get; } = new List<CompareLogItemViewModel>();

        public event EventHandler<CloseRequestedEventArgs> CloseRequested;

        public ICommand CloseCommand
        {
            get;
        }
        public ICommand SetVisibilityCommand { get; }

        public ObservableCollection<CompareLogItemViewModel> Logs { get; set; } = new ObservableCollection<CompareLogItemViewModel>();

        public bool ShowDifferencesOnly
        {
            get => _showDifferencesOnly;
            set
            {
                Set(ref _showDifferencesOnly, value);
                Logs.Clear();
                var l = value ? _filteredLogs : _completeLogs;
                foreach (var item in l)
                {
                    Logs.Add(item);
                    item.Checked = item.Level == 0;
                    item.Visible = item.Level < 2;
                }
            }
        }

        public CompareResultViewModel()
        {
            CloseCommand = new RelayCommand(Close_Executed);
            SetVisibilityCommand = new RelayCommand<CompareLogItemViewModel>(SetVisibility);
        }

        private void SetVisibility(CompareLogItemViewModel item)
        {
            var index = Logs.IndexOf(item);
            if (item.Checked)
            {
                foreach (var l in Logs.Skip(index + 1).TakeWhile(c => c.Level > item.Level).Where(c => c.Level == item.Level + 1))
                {
                    l.Visible = true;
                    if (l.Checked)
                    {
                        var index1 = Logs.IndexOf(l);
                        foreach (var lo in Logs.Skip(index1 + 1).TakeWhile(c => c.Level > l.Level))
                        {
                            lo.Visible = true;
                        }
                    }
                }
            }
            else
            {
                foreach (var l in Logs.Skip(index + 1).TakeWhile(c => c.Level > item.Level))
                {
                    l.Visible = false;
                }
            }
        }

        private void Close_Executed()
        {
            CloseRequested?.Invoke(this, new CloseRequestedEventArgs(true));
        }

        public void AddComparisonResult(IEnumerable<CompareLogModel> logs)
        {
            CompareLogItemViewModel Create(CompareLogModel log, int level)
            {
                return new CompareLogItemViewModel
                {
                    Visible = level == 0,
                    Attribute = log.Attribute,
                    Expected = log.State == CompareState.Ok ? null : log.Expected,
                    Found = log.Found,
                    Level = level,
                    Name = log.Name,
                    State = log.State,
                    Type = log.Type,
                    HasChildren = log.SubLogs.Any() ? (bool?)true : null
                };
            }

            foreach (var ctx in logs)
            {
                _completeLogs.Add(Create(ctx, 0));
                foreach (var entity in ctx.SubLogs)
                {
                    _completeLogs.Add(Create(entity, 1));
                    foreach (var property in entity.SubLogs)
                    {
                        _completeLogs.Add(Create(property, 2));
                    }
                }
            }

            var clonedLogs = JsonConvert.DeserializeObject<List<CompareLogModel>>(JsonConvert.SerializeObject(logs));
            foreach (var ctx in clonedLogs)
            {
                foreach (var entity in ctx.SubLogs)
                {
                    entity.SubLogs.RemoveAll(c => c.State == CompareState.Ok);
                }
                ctx.SubLogs.RemoveAll(c => c.State == CompareState.Ok && c.SubLogs.All(s => s.State == CompareState.Ok));
            }
            clonedLogs.RemoveAll(c => c.State == CompareState.Ok && c.SubLogs.All(s => s.State == CompareState.Ok));
            foreach (var ctx in clonedLogs)
            {
                _filteredLogs.Add(Create(ctx, 0));
                foreach (var entity in ctx.SubLogs)
                {
                    _filteredLogs.Add(Create(entity, 1));
                    foreach (var property in entity.SubLogs)
                    {
                        _filteredLogs.Add(Create(property, 2));
                    }
                }
            }

            foreach (var item in _filteredLogs)
            {
                Logs.Add(item);
            }
        }
    }
}