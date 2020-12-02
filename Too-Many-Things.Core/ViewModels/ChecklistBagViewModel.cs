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

namespace Too_Many_Things.Core.ViewModels
{
    public class ChecklistBagViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "ChecklistBag";
        public IScreen HostScreen { get; }
        private IChecklistService _checklistService;

        // List of checklists that should be bound to.
        private IObservableList<Checklist> _checklistList;
        public IObservableList<Checklist> ChecklistList
        {
            get => _checklistList;
            set => this.RaiseAndSetIfChanged(ref _checklistList, value);
        }

        public Checklist SelectedChecklist;

        public ChecklistBagViewModel(IScreen screen = null, IChecklistService checklistService = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            _checklistService = checklistService ?? Locator.Current.GetService<IChecklistService>();

            // Makes Selected checklist into an observable property.
            IObservable<Checklist> selectedChecklist = this.WhenAnyValue(x => x.SelectedChecklist);

            // TODO : Is this pointless?
            var baseList = _checklistService.GetLocalCollectionSource();
            _checklistList = baseList.ToObservableChangeSet()
                .AsObservableList();
        }
    }
}
