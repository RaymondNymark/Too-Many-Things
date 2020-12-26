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
using System.Threading.Tasks;

namespace Too_Many_Things.Core.ViewModels
{
    public class SecondaryViewModel : ReactiveObject, IRoutableViewModel
    {
        #region Reactive Commands & Interactions
        public ReactiveCommand<Unit, IRoutableViewModel> GoBackToPrimaryView { get; }
        #endregion

        #region Properties
        private ChecklistDataService _checklistService;
        public string UrlPathSegment => "Secondary";
        public IScreen HostScreen { get; }
        // Flag to keep track of if Edit mode is enabled.
        [Reactive]
        public bool EditModeIsEnabled { get; set; } = false;
        // List to bind ItemSource to.
        [Reactive]
        public List<EntryViewModel> BindingEntryCache { get; set; } = new List<EntryViewModel>();

        private List _selectedList { get; set; }

        #endregion
        public SecondaryViewModel(List selectedList, IScreen screen = null, ChecklistDataService checklistService = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            _checklistService = checklistService ?? Locator.Current.GetService<ChecklistDataService>();
            _selectedList = selectedList;

            GoBackToPrimaryView = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new PrimaryViewModel(HostScreen, null)));
            UpdateBindingEntryCacheAsync();
        }

        #region Methods and Tasks

        /// <summary>
        /// Refreshes BindingEntryCache with current database elements.
        /// </summary>
        public async Task UpdateBindingEntryCacheAsync()
        {
            var derivedCache = new List<EntryViewModel>();
            var freshCache = await _checklistService.LoadEntryDataAsync(_selectedList);

            foreach(Entry entry in freshCache)
            {
                derivedCache.Add(new EntryViewModel(entry, entry.EntryID, entry.Name, entry.IsChecked, entry.IsDeleted, entry.SortOrder, entry.ListID));
            }

            BindingEntryCache = derivedCache;
        }
        #endregion
    }
}
