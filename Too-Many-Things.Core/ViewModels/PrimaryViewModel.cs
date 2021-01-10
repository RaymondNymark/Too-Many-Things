using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using Too_Many_Things.Core.DataAccess.Models;
using Too_Many_Things.Core.Services;
using static Too_Many_Things.Core.Enums.Enums;

namespace Too_Many_Things.Core.ViewModels
{
    public class PrimaryViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Primary";
        public IScreen HostScreen { get; }

        private IChecklistDataService _checklistService;

        #region Reactive Commands & Interactions
        public ReactiveCommand<Unit, Unit> RefreshCommand { get; }
        public ReactiveCommand<Unit, Unit> NewDefaultChecklistCommand { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> OpenList { get; }
        public ReactiveCommand<Unit, Unit> ConfirmRenameCommand { get; }
        public ReactiveCommand<Unit, Unit> ConfirmDeletionCommand { get; }

        public ReactiveCommand<Unit, Unit> CancelEditCommand { get; }

        public ReactiveCommand<InterfaceState, Unit> EnableEditCommand { get; }
        #endregion

        public PrimaryViewModel(IScreen screen = null, IChecklistDataService checklistService = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();

            InitializeApp(checklistService);

            // TODO : Cleaner way to automatically refresh when settings is closed or once it's connected?
            RefreshCommand = ReactiveCommand.Create(() => InitializeApp(null));

            // New Default Checklist command:
            var connectSaveCanExecute = this.WhenAnyValue(
                x => x.IsConfigured,
                (flag) => flag == true);
            NewDefaultChecklistCommand = ReactiveCommand.CreateFromTask(() => NewDefaultChecklist(), connectSaveCanExecute);

            OpenList = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new SecondaryViewModel(SelectedList.List, HostScreen, _checklistService)));

            #region Edit mode (Mainly re-name and deletion)
            var renameCanExecute = this.WhenAnyValue(
                x => x.RenameListInput,
                (input) => input.Length > 0);
            // Change button template style so it looks obvious to user that
            // they can click a button. input.Length > 0 enables button.

            var EnableEditCanExecute = this.WhenAnyValue(
                x => x.SelectedList,
                (selected) => (ListViewModel)selected != null);


            EnableEditCommand = ReactiveCommand.Create((InterfaceState state) => EnableEdit(state), EnableEditCanExecute);
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
        [Reactive]
        public bool IsRenaming { get; set; } = false;
        [Reactive]
        public bool IsDeleting { get; set; } = false;
        [Reactive]
        public double GridOppacity { get; set; } = 1;
        [Reactive]
        public string ChangeNameString { get; set; } = "Change the name of an entry";
        [Reactive]
        public string DeleteObjectString { get; set; } = "Are you sure you want to delete?";

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
        private async void InitializeApp(IChecklistDataService checklistService)
        {
            var isConfigured = ConnectionStringManager.ConnectionIsConfigured();

            if (isConfigured)
            {
                // Big guns - Retrieves the service from DI.
                ConfigurationStatus = string.Empty;

                _checklistService = checklistService ?? Locator.Current.GetService<IChecklistDataService>();

                await LoadDatabaseDataIntoMemoryAsync();

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
                    ChangeNameString = $"Change the name of {SelectedList.Name}";
                    break;
                case InterfaceState.Deleting:
                    InterfaceState = InterfaceState.Deleting;
                    DeleteObjectString = $"Are you sure you want to delete {SelectedList.Name} ?";
                    break;
            }
        }

        /// <summary>
        /// Renames a list Asynchronously to a new name by renaming the object
        /// in memory and tells IChecklistDataService to rename the object in
        /// database without blocking the ui-thread.
        /// </summary>
        public async Task RenameListAsync()
        {
            // Retrieves the name the user input into the rename-textbox.
            var newName = RenameListInput;

            // Ra-naming the checklist in memory, and returns the interface to a default state.
            SelectedList.Name = RenameListInput;
            SelectedList.List.Name = RenameListInput;
            InterfaceState = InterfaceState.Default;

            // Updates the checklist's name in the database to the new name in the background.
            await _checklistService.UpdateChecklistNameAsync(SelectedList.List, RenameListInput);
        }

        /// <summary>
        /// Removes the selected checklist object from memory and sends request
        /// to databaseService class to soft-delete the checklist.
        /// </summary>
        public async Task DeleteListAsync()
        {
            //Soft Deleting the checklist:
            await _checklistService.SoftDeleteChecklistAsync(SelectedList.List);
            // Sets interface back to default view. 
            InterfaceState = InterfaceState.Default;
            await LoadDatabaseDataIntoMemoryAsync();
        }

        /// <summary>
        /// Returns the interface state back to default and removes edit ui.
        /// </summary>
        public void CancelEdit()
        {
            InterfaceState = InterfaceState.Default;
        }

        /// <summary>
        /// Adds a new default checklist to the database and forces a refresh of the content.
        /// </summary>
        public async Task NewDefaultChecklist()
        {
            await _checklistService.AddDefaultChecklistAsync();
            await LoadDatabaseDataIntoMemoryAsync();
        }

        /// <summary>
        /// Loads all of the database data into memory and converts the list of
        /// model to ViewModel so it can be manipulated and displayed in the
        /// application.
        /// </summary>
        private async Task LoadDatabaseDataIntoMemoryAsync()
        {
            var UsingSqlDataBase = true;

            if (UsingSqlDataBase)
            {
                var retrievedCache = await _checklistService.LoadFullListData();
                // Converts the database model into a modelViewModel.
                var convertedCache = ConvertModelToViewModel(retrievedCache);

                // Updates the BindingCache which the application uses to
                // display and let the user interact data with.
                BindingCache = convertedCache;
            }
            else
            {
                // Implement non-database functionality here for 1.1 release.
            }
        }

        /// <summary>
        /// Converts a list of 'Lists' into a list of 'ListViewModels'.
        /// </summary>
        /// <param name="">List that shall be converted</param>
        /// <returns>The converted list</returns>
        private static List<ListViewModel> ConvertModelToViewModel(List<List> inputList)
        {
            var convertedList = new List<ListViewModel>();

            foreach (List list in inputList)
            {
                convertedList.Add(new ListViewModel(list));
            }

            return convertedList;
        }
        #endregion
    }
}
