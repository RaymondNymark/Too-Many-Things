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
using System.Threading.Tasks;
using System.Reactive.Linq;
using Too_Many_Things.Core.DataAccess.Models;
using ReactiveUI.Fody.Helpers;
using static Too_Many_Things.Core.Enums.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Too_Many_Things.Core.ViewModels
{
    public class PrimaryViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Primary";
        public IScreen HostScreen { get; }

        private ChecklistDataService _checklistService;

        #region Reactive Commands & Interactions
        public ReactiveCommand<Unit, Unit> RefreshCommand { get; }
        public ReactiveCommand<Unit, Unit> NewDefaultChecklistCommand { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> OpenList { get; }
        public ReactiveCommand<Unit, Unit> ConfirmRenameCommand { get; }
        public ReactiveCommand<Unit, Unit> ConfirmDeletionCommand { get; }

        public ReactiveCommand<Unit, Unit> CancelEditCommand { get; }

        public ReactiveCommand<InterfaceState, Unit> EnableEditCommand { get; }
        #endregion

        public PrimaryViewModel(IScreen screen = null, ChecklistDataService checklistService = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            InitializeApp(checklistService);

            // TODO : Cleaner way to automatically refresh when settings is closed or once it's connected?
            RefreshCommand = ReactiveCommand.Create(() => InitializeApp(null));

            // New Default Checklist command:
            var connectSaveCanExecute = this.WhenAnyValue(
                x => x.IsConfigured,
                (flag) => flag == true);
            NewDefaultChecklistCommand = ReactiveCommand.CreateFromTask(() => _checklistService.AddDefaultChecklist(), connectSaveCanExecute);

            OpenList = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new EntriesViewModel(SelectedList.List, HostScreen, _checklistService)));

            #region Edit mode (Mainly re-name and deletion)
            var renameCanExecute = this.WhenAnyValue(
                x => x.RenameListInput,
                (input) => input.Length > 0);
            // Change button template style so it looks obvious to user that
            // they can click a button. input.Length > 0 enables button.
            
            EnableEditCommand = ReactiveCommand.Create((InterfaceState state) => EnableEdit(state));
            ConfirmRenameCommand = ReactiveCommand.CreateFromTask(() => RenameListAsync(), renameCanExecute);
            ConfirmDeletionCommand = ReactiveCommand.CreateFromTask(() => DeleteListAsync());
            CancelEditCommand = ReactiveCommand.Create(() => CancelEdit());
            #endregion
        }

        #region Properties
        [Reactive]
        public List<ListViewModel> BindingCache { get; set; } = new List<ListViewModel>();
        [Reactive]
        public bool IsConfigured { get; set; }
        [Reactive]
        public string ConfigurationStatus { get; set; }
        [Reactive]
        public ObservableCollection<List> ChecklistCache { get; set; }
        [Reactive]
        public ListViewModel SelectedList { get; set; }
        [Reactive]
        public string RenameListInput { get; set; } = string.Empty;
        // Quick hacks
        [Reactive]
        public bool IsRenaming { get; set; } = false;
        [Reactive]
        public bool IsDeleting { get; set; } = false;
        [Reactive]
        public double GridOppacity { get; set; } = 1;

        // Silly properties because of how command params work.
        public InterfaceState RenamingState = InterfaceState.Renaming;
        public InterfaceState DeletingState = InterfaceState.Deleting;

        // Property for InterfaceState.
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
                        RenameListInput = "";
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
        /// <summary>
        /// Sets up the primary view to be able to be used if a database
        /// connection has been configured. Otherwise it asks user to configure
        /// it.
        /// </summary>
        private async void InitializeApp(ChecklistDataService checklistService)
        {
            var isConfigured = ConnectionStringManager.ConnectionIsConfigured();

            if (isConfigured)
            {
                // Big guns - Retrieves the service from DI.
                ConfigurationStatus = string.Empty;

                _checklistService = checklistService ?? Locator.Current.GetService<ChecklistDataService>();

                await UpdateBindingCache();

                // Flips IsConfigured flag.
                IsConfigured = true;
            }
            else
            {
                // Small guns
                // Changes configuration Status text to inform user that the
                // database hasn't been initialized.
                ConfigurationStatus = "A database has not been configured. Please configure one in the settings. :)";

                // Unflips flag.
                IsConfigured = false;
            }
        }

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

        public async Task RenameListAsync()
        {
            // Ra-naming the checklist.
            await _checklistService.UpdateChecklistNameAsync(SelectedList.List, RenameListInput);
            InterfaceState = InterfaceState.Default;
        }

        /// <summary>
        /// Soft deletes the selected list.
        /// </summary>
        public async Task DeleteListAsync()
        {
            // Soft Deleting the checklist:
            await _checklistService.SoftDeleteChecklistAsync(SelectedList.List);
            // Sets interface back to default view. 
            InterfaceState = InterfaceState.Default;
        }

        /// <summary>
        /// Returns the interface state back to default and removes edit ui.
        /// </summary>
        public void CancelEdit()
        {
            InterfaceState = InterfaceState.Default;
        }

        private ObservableCollection<List> RetrieveLocalSource()
        {
            return _checklistService.GetLocal();
        }

        private async Task UpdateBindingCache()
        {
            var derivedCache = new List<ListViewModel>();
            var refreshCache = await _checklistService.LoadDataAsync();

            foreach(List list in refreshCache)
            {
                derivedCache.Add(new ListViewModel(list, list.ListID, list.Name, list.IsDeleted, list.SortOrder, list.Entries));
            }

            // TODO : Sort by order.
            BindingCache = derivedCache;
        }
        #endregion
    }
}
