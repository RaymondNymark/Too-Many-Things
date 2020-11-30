using System;
using System.Collections.Generic;
using System.Text;
using Too_Many_Things.Core.Services;
using Too_Many_Things.Core.Models;
using Too_Many_Things.Core.ViewModels;
using System.Collections.ObjectModel;
using ReactiveUI;
using Splat;

namespace Too_Many_Things.Core.ViewModels
{
    public class ChecklistBagViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "ChecklistBag";
        public IScreen HostScreen { get; }
        private IChecklistService _checklistService;

        public ChecklistBagViewModel(IChecklistService checklistService = null)
        {
            _checklistService = checklistService ?? Locator.Current.GetService<IChecklistService>();
        }
    }
}
