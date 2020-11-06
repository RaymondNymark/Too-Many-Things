using System;
using System.Collections.Generic;
using System.Text;
using Too_Many_Things.Services;
using Too_Many_Things.Models;
using Too_Many_Things.ViewModels;
using System.Collections.ObjectModel;

namespace Too_Many_Things.ViewModels
{
    public class OuterViewModel : ViewModelBase
    {
        private IChecklistService _checklistService;
        private Checklist _selectedChecklist;

        public OuterViewModel(IChecklistService checklistService)
        {
            _checklistService = checklistService;
        }

        #region Data-Binding properties

        // Retrieves collection of checklist to bind to local ItemSource.
        public ObservableCollection<Checklist> ItemSource
        {
            get => _checklistService.GetLocalView();
        }

        // Keeps track of which checklist is 'selected' or 'clicked' or 'focused'.
        public Checklist SelectedChecklist
        {
            get => _selectedChecklist;
            set => SetProperty(ref _selectedChecklist, value);
        }
        #endregion
    }
}
