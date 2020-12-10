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
        ReactiveCommand<Unit, IRoutableViewModel> CloseChecklist { get; }
        ObservableCollection<Entry> EntryList { get; }
    }
    public class EntryViewModel : ReactiveObject, IRoutableViewModel, IEntryViewModel
    {
        public string UrlPathSegment => "EntryBag";
        public IScreen HostScreen { get; }
        private IChecklistService _checklistService;

        // Main Checklist
        private Checklist _headChecklist;
        public Checklist HeadChecklist
        {
            get => _headChecklist;
            set => this.RaiseAndSetIfChanged(ref _headChecklist, value);
        }

        // ItemCollection to bind to.
        private ObservableCollection<Entry> _entryList;

        public ObservableCollection<Entry> EntryList
        {
            get => _entryList;
            set => this.RaiseAndSetIfChanged(ref _entryList, value);
        }

        public EntryViewModel(int checklistID, IScreen screen = null, IChecklistService checklistService = null)
        {
           HostScreen = screen ?? Locator.Current.GetService<IScreen>();
           _checklistService = checklistService ?? Locator.Current.GetService<IChecklistService>();

            //var currentChecklist = _checklistService.Get(checklistID);


            EntryList = _checklistService.GetEntriesFromChecklist(checklistID);

            CloseChecklist = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new ChecklistBagViewModel(HostScreen, null)));
        }

        #region Command-region
        public ReactiveCommand<Unit, IRoutableViewModel> CloseChecklist { get; }
        #endregion
    }
}
