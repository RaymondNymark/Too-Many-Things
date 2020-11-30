using System;
using System.Collections.Generic;
using System.Text;
using Too_Many_Things.Core.Services;
using Too_Many_Things.Core.Models;
using Too_Many_Things.Core.ViewModels;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace Too_Many_Things.Core.ViewModels
{
    public class OuterViewModel : ReactiveObject
    {
        private IChecklistService _checklistService;
        private Checklist _selectedChecklist;

        public OuterViewModel(IScreen hostScreen, IChecklistService checklistService)
        {
            _checklistService = checklistService;


        }



    }
}
