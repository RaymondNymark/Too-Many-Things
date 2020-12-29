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
using static Too_Many_Things.Core.Enums.Enums;

namespace Too_Many_Things.Core.ViewModels
{
    public class SecondaryViewModel : ReactiveObject, IRoutableViewModel
    {
        #region Reactive Commands & Interactions
        public ReactiveCommand<Unit, IRoutableViewModel> GoBackToPrimaryView { get; }
        public ReactiveCommand<InterfaceState, Unit> EnableEditCommand { get; }
        public ReactiveCommand<Unit, Unit> ConfirmRenameCommand { get; }
        public ReactiveCommand<Unit, Unit> ConfirmDeletionCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelEditCommand { get; }

        #endregion
        public SecondaryViewModel(List selectedList, IScreen screen = null, IChecklistDataService checklistService = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            _checklistService = checklistService ?? Locator.Current.GetService<IChecklistDataService>();
            SelectedList = selectedList;

            GoBackToPrimaryView = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new PrimaryViewModel(HostScreen, null)));
            Initialize();

            #region Edit mode (Re-name and deletion)
            var renameCanExecute = this.WhenAnyValue(
                x => x.RenameEntryInput,
                (input) => input.Length > 0);

            EnableEditCommand = ReactiveCommand.Create((InterfaceState state) => EnableEdit(state));
            ConfirmRenameCommand = ReactiveCommand.CreateFromTask(() => RenameEntryAsync(), renameCanExecute);
            ConfirmDeletionCommand = ReactiveCommand.CreateFromTask(() => DeleteEntryAsync());
            CancelEditCommand = ReactiveCommand.Create(() => CancelEdit());
            #endregion
        }

        #region Properties
        private IChecklistDataService _checklistService;
        public string UrlPathSegment => "Secondary";
        public IScreen HostScreen { get; }
        // Flag to keep track of if Edit mode is enabled.
        [Reactive]
        public bool EditModeIsEnabled { get; set; } = false;
        // List to bind ItemSource to.
        [Reactive]
        public List<EntryViewModel> BindingEntryCache { get; set; } = new List<EntryViewModel>();
        [Reactive]
        public List SelectedList { get; set; }

        [Reactive]
        public EntryViewModel SelectedEntry { get; set; }

        // ---All of the edit mode properties---
        [Reactive]
        public string RenameEntryInput { get; set; } = string.Empty;
        [Reactive]
        public bool IsRenaming { get; set; } = false;
        [Reactive]
        public bool IsDeleting { get; set; } = false;
        [Reactive]
        public double GridOppacity { get; set; } = 1;

        // Silly properties that need to be here to give commands parameters..
        public InterfaceState RenamingState = InterfaceState.Renaming;
        public InterfaceState DeletingState = InterfaceState.Deleting;

        // Property for this SecondView's interface state.
        private InterfaceState _interfaceState;
        public InterfaceState InterfaceState
        {
            get => _interfaceState;
            set
            {
                switch (value)
                {
                    case InterfaceState.Default:
                        IsRenaming = false;
                        IsDeleting = false;
                        GridOppacity = 1.0;
                        break;
                    case InterfaceState.Renaming:
                        IsRenaming = true;
                        IsDeleting = false;
                        RenameEntryInput = "";
                        GridOppacity = 0.3;
                        break;
                    case InterfaceState.Deleting:
                        IsRenaming = false;
                        IsDeleting = true;
                        GridOppacity = 0.3;
                        break;
                }
                this.RaiseAndSetIfChanged(ref _interfaceState, value);
            }
        }
        #endregion

        #region Asynchronous Tasks & Methods
        private async void Initialize()
        {
            await UpdateBindingEntryCacheAsync();
        }

        /// <summary>
        /// Refreshes BindingEntryCache with current database elements.
        /// </summary>
        private async Task UpdateBindingEntryCacheAsync()
        {
            var derivedCache = new List<EntryViewModel>();
            var freshCache = await _checklistService.LoadEntryDataAsync(SelectedList);

            foreach(Entry entry in freshCache)
            {
                derivedCache.Add(new EntryViewModel(entry, entry.EntryID, entry.Name, entry.IsChecked, entry.IsDeleted, entry.SortOrder, entry.ListID, _checklistService));
            }

            BindingEntryCache = derivedCache;
        }

        /// <summary>
        /// This method flips all switches internally to change the InterfaceState by input. 
        /// </summary>
        /// <param name="state">Which state to switch to.</param>
        private void EnableEdit(InterfaceState state)
        {
            switch (state)
            {
                case InterfaceState.Renaming:
                    InterfaceState = InterfaceState.Renaming;
                    break;
                case InterfaceState.Deleting:
                    InterfaceState = InterfaceState.Deleting;
                    break;
            }
        }

        /// <summary>
        /// Renames the Selected Entry to a new name.
        /// </summary>
        public async Task RenameEntryAsync()
        {
            await _checklistService.RenameEntryAsync(SelectedEntry.Entry, RenameEntryInput);
            await UpdateBindingEntryCacheAsync();
            InterfaceState = InterfaceState.Default;
        }

        /// <summary>
        /// Deletes the selected Entry.
        /// </summary>
        public async Task DeleteEntryAsync()
        {
            await _checklistService.DeleteEntryAsync(SelectedEntry.Entry);
            await UpdateBindingEntryCacheAsync();
            InterfaceState = InterfaceState.Default;
        }

        /// <summary>
        /// Returns the interface state back to default and removes edit UI.
        /// </summary>
        public void CancelEdit()
        {
            InterfaceState = InterfaceState.Default;
        }
        #endregion
    }
}
