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
    public interface IChecklistBagViewModel
    {
        ObservableCollection<Checklist> ChecklistList { get; set; }
        IScreen HostScreen { get; }
        ReactiveCommand<Unit, IRoutableViewModel> OpenChecklist { get; }
        ReactiveCommand<Unit, SettingsViewModel> OpenSettings { get; }
        Checklist SelectedChecklist { get; set; }
        string UrlPathSegment { get; }
    }

    public class ChecklistBagViewModel : ReactiveObject, IRoutableViewModel, IChecklistBagViewModel
    {
        public string UrlPathSegment => "ChecklistBag";
        public IScreen HostScreen { get; }

        private IChecklistService _checklistService;

        public ObservableCollection<Checklist> TestItemSource = new ObservableCollection<Checklist>();


        // List of checklists that should be bound to.
        // TODO : USE DYNAMIC DATA!!!
        private ObservableCollection<Checklist> _checklistList;
        public ObservableCollection<Checklist> ChecklistList
        {
            get => _checklistList;
            set => this.RaiseAndSetIfChanged(ref _checklistList, value);
        }

        private Checklist _selectedChecklist;

        public Checklist SelectedChecklist
        {
            get => _selectedChecklist;
            set => this.RaiseAndSetIfChanged(ref _selectedChecklist, value);
        }


        public ChecklistBagViewModel(IScreen screen = null, IChecklistService checklistService = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            _checklistService = checklistService ?? Locator.Current.GetService<IChecklistService>();
            _checklistList = _checklistService.GetLocalCollectionSource();


            OpenChecklist = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new EntryViewModel(SelectedChecklist, HostScreen, _checklistService)));
            OpenSettings = ReactiveCommand.Create(() => new SettingsViewModel());
        }

        #region Command-region
        public ReactiveCommand<Unit, IRoutableViewModel> OpenChecklist { get; }
        public ReactiveCommand<Unit, SettingsViewModel> OpenSettings { get; }
        #endregion
    }
}
