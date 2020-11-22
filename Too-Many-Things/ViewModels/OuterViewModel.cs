using System;
using System.Collections.Generic;
using System.Text;
using Too_Many_Things.Services;
using Too_Many_Things.Models;
using Too_Many_Things.ViewModels;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace Too_Many_Things.ViewModels
{
    public interface IOuterViewModel : IRoutableViewModel
    {

    }
    public class OuterViewModel : ReactiveObject, IOuterViewModel
    {
        public string UrlPathSegment
        {
            get => "outer";
        }
        public IScreen HostScreen { get; protected set; }

        private IChecklistService _checklistService;
        private Checklist _selectedChecklist;

        public OuterViewModel(IScreen hostScreen, IChecklistService checklistService)
        {
            HostScreen = hostScreen;
            _checklistService = checklistService;
        }

        #region Data-Binding properties

        // Keeps track of which checklist is 'selected' or 'clicked' or 'focused'.
        public Checklist SelectedChecklist
        {
            get => _selectedChecklist;
            set => this.RaiseAndSetIfChanged(ref _selectedChecklist, value);
        }

        public ObservableCollection<Checklist> ItemSource
        {
            get => _checklistService.GetLocalCollectionSource();
        }
        #endregion
    }
}
