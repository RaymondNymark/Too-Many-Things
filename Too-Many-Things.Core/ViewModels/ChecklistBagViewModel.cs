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
        Checklist SelectedChecklist { get; set; }
    }

    public class ChecklistBagViewModel : ReactiveObject, IRoutableViewModel, IChecklistBagViewModel
    {
        public string UrlPathSegment => "ChecklistBag";
        public IScreen HostScreen { get; }
        private IChecklistService _checklistService;

        public ObservableCollection<Checklist> TestItemSource = new ObservableCollection<Checklist>();
        public string PublicFreeString = "String from space";

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

            // Makes Selected checklist into an observable property.
            //IObservable<Checklist> selectedChecklist = this.WhenAnyValue(x => x.SelectedChecklist);

            // TODO : Is this pointless?
            _checklistList = _checklistService.GetLocalCollectionSource();
        }
    }
}
