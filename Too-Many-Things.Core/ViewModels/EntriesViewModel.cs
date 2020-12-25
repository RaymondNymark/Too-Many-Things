using System;
using System.Collections.Generic;
using System.Text;
using Too_Many_Things.Core.Services;
using Too_Many_Things.Core.ViewModels;
using System.Collections.ObjectModel;
using ReactiveUI;
using Splat;
using DynamicData;
using DynamicData.Binding;
using System.Reactive;
using System.Diagnostics;
using ReactiveUI.Fody.Helpers;
using Too_Many_Things.Core.DataAccess.Models;

namespace Too_Many_Things.Core.ViewModels
{
    public interface IEntryViewModel
    {
        ReactiveCommand<Unit, IRoutableViewModel> CloseChecklist { get; }
        ObservableCollection<Entry> EntryList { get; }
    }
    public class EntriesViewModel : ReactiveObject, IRoutableViewModel, IEntryViewModel
    {
        #region Properties
        public string UrlPathSegment => "Entries";
        public IScreen HostScreen { get; }
        private ChecklistDataService _checklistService;

        // Collection syphon from entries..
        [Reactive]
        public ObservableCollection<Entry> EntryList { get; set; }
        // Flag to enable / disable edit mode.
        [Reactive]
        public bool EditModeEnabled { get; set; }

        #endregion

        #region Command-region
        public ReactiveCommand<Unit, IRoutableViewModel> CloseChecklist { get; }
        #endregion

        public EntriesViewModel(List selectedChecklist, IScreen screen = null, ChecklistDataService checklistService = null)
        {
           HostScreen = screen ?? Locator.Current.GetService<IScreen>();
           _checklistService = checklistService ?? Locator.Current.GetService<ChecklistDataService>();

            EntryList = selectedChecklist.Entries;

            CloseChecklist = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new PrimaryViewModel(HostScreen, null)));
        }   
    }
}
