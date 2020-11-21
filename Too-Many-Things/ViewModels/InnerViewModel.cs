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
    public interface IInnerViewModel : IRoutableViewModel
    {

    }
    public class InnerViewModel : ReactiveObject, IInnerViewModel
    {
        public string UrlPathSegment
        {
            get => "inner";
        }
        public IScreen HostScreen { get; protected set; }
        private IChecklistService _checklistService;

        public InnerViewModel(IScreen hostScreen, IChecklistService checklistService)
        {
            HostScreen = hostScreen;
            _checklistService = checklistService;
        }
    }
}
