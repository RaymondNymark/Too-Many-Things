using System;
using System.Collections.Generic;
using System.Text;
using Too_Many_Things.Core.Services;
using Too_Many_Things.Core.Models;
using Too_Many_Things.Core.ViewModels;
using System.Collections.ObjectModel;
using ReactiveUI;
using Splat;
using DynamicData;
using DynamicData.Binding;
using System.Reactive;
using System.Diagnostics;
namespace Too_Many_Things.Core.ViewModels
{
    public interface IEntryViewModel
    {

    }
    public class EntryViewModel : ReactiveObject, IRoutableViewModel, IEntryViewModel
    {
        public string UrlPathSegment => "EntryBag";
        public IScreen HostScreen { get; }
        private IChecklistService _checklistService;

        // ItemCollection to bind to.
        private ObservableCollection<Entry> _entryList;
        public ObservableCollection<Entry> EntryListBindable
        {
            get => _entryList;
            set => this.RaiseAndSetIfChanged(ref _entryList, value);
        }

        public EntryViewModel(Checklist checklist, IScreen screen = null, IChecklistService checklistService = null)
        {
           HostScreen = screen ?? Locator.Current.GetService<IScreen>();
           _checklistService = checklistService ?? Locator.Current.GetService<IChecklistService>();

            _entryList = checklist.Entry;
        }
    }
}
