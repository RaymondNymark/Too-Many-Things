using System.Collections.Generic;
using Too_Many_Things.Core.Services;
using ReactiveUI;
using Splat;
using System.Reactive;
using ReactiveUI.Fody.Helpers;
using Too_Many_Things.Core.DataAccess.Models;
using System.Threading.Tasks;
using static Too_Many_Things.Core.Enums.Enums;
using System.Collections.ObjectModel;
using System.Linq;

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

        public ReactiveCommand<Unit, Unit> TurnEntryEditOnCommand { get; }
        public ReactiveCommand<Unit, Unit> TurnEntryEditOffCommand { get; }
        public ReactiveCommand<Unit, Unit> NewDefaultEntryCommand { get; }

        public ReactiveCommand<bool, Unit> CheckEveryEntry { get; }
        #endregion
        public SecondaryViewModel(List selectedList, IScreen screen = null, IChecklistDataService checklistService = null, ILocalDataStorageService localDataStorageService = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            _checklistService = checklistService ?? Locator.Current.GetService<IChecklistDataService>();
            _localDataStorageService = localDataStorageService ?? Locator.Current.GetService<ILocalDataStorageService>();

            SelectedList = selectedList;

            GoBackToPrimaryView = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new PrimaryViewModel(HostScreen, null)));
            Initialize();

            #region Edit mode (Re-name and deletion)
            // Can execute statements.
            var renameCanExecute = this.WhenAnyValue(
                x => x.RenameEntryInput,
                (input) => input.Length > 0);

            var EnableEditCanExecute = this.WhenAnyValue(
                x => x.SelectedEntry,
                (selected) => (EntryViewModel)selected != null);


            EnableEditCommand = ReactiveCommand.Create((InterfaceState state) => EnableEdit(state), EnableEditCanExecute);
            ConfirmRenameCommand = ReactiveCommand.CreateFromTask(() => RenameEntryAsync(), renameCanExecute);
            ConfirmDeletionCommand = ReactiveCommand.CreateFromTask(() => DeleteEntryAsync());
            CancelEditCommand = ReactiveCommand.Create(() => CancelEdit());
            NewDefaultEntryCommand = ReactiveCommand.CreateFromTask(() => AddNewDefaultEntryAsync());

            TurnEntryEditOnCommand = ReactiveCommand.Create(() => 
            {
                EditModeIsEnabled = true;
                EditModeIsDisabled = false;
            });
            TurnEntryEditOffCommand = ReactiveCommand.Create(() =>
            {
                EditModeIsEnabled = false;
                EditModeIsDisabled = true;
            });
            #endregion

            CheckEveryEntry = ReactiveCommand.CreateFromTask((bool whatToMarkAs) => MarkCurrentCollectionIsChecked(whatToMarkAs));
        }

        #region Properties
        private bool _usingSqlDataBase = false; // TODO
        private ObservableCollection<List> BindingCollection;
        private ILocalDataStorageService _localDataStorageService;
        private IChecklistDataService _checklistService;
        public string UrlPathSegment => "Secondary";
        public IScreen HostScreen { get; }
        // Flag to keep track of if Edit mode is enabled.
        // Silly way to deal with this.
        [Reactive]
        public bool EditModeIsEnabled { get; set; } = false;
        [Reactive]
        public bool EditModeIsDisabled { get; set; } = true;
        // List to bind ItemSource to.
        [Reactive]
        public ObservableCollection<EntryViewModel> BindingEntryCache { get; set; } = new ObservableCollection<EntryViewModel>();
        [Reactive]
        public List SelectedList { get; set; }

        [Reactive]
        public EntryViewModel SelectedEntry { get; set; }


        // ---All of the edit mode properties---
        [Reactive]
        public string ChangeNameString { get; set; } = "Change the name of an entry";
        [Reactive]
        public string DeleteObjectString { get; set; } = "Are you sure you want to delete?";

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
        public bool TrueBool = true;
        public bool FalseBool = false;

        // Property for this SecondView's interface state.
        private InterfaceState _interfaceState;
        private InterfaceState InterfaceState
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
        /// <summary>
        /// Initializes necessary components to make this class work. 
        /// </summary>
        private async void Initialize()
        {
            // void return in order to have Async methods in class constructor. T.T
            await LoadDataIntoMemoryAsync();
        }
        
        /// <summary>
        /// Loads all of the database data related to the selected checklist
        /// that was opened into memory. Then converts it to a list of
        /// EntryViewModel so it can be manipulated and displayed in the
        /// application.
        /// </summary>
        private async Task LoadDataIntoMemoryAsync()
        {
            ObservableCollection<Entry> RetrievedCache;
            ObservableCollection<EntryViewModel> ConvertedCache;

            if (_usingSqlDataBase)
            {
                // Retrieves the SQL Entry data related to the checklist that was opened and converts it from List T.
                RetrievedCache = new ObservableCollection<Entry>(await _checklistService.LoadEntryDataAsync(SelectedList));
            }
            else
            {
                // Retrieves the locally stored data from LocalDataStorageService.
                BindingCollection = await _localDataStorageService.RetrieveStoredObjectAsync();
                var target = BindingCollection.FirstOrDefault(x => x.ListID == SelectedList.ListID);

                RetrievedCache = target.Entries;
            }

            ConvertedCache = ConvertModelToViewModel(RetrievedCache);
            BindingEntryCache = ConvertedCache;
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
                    ChangeNameString = $"Change the name of {SelectedEntry.Name}";
                    break;
                case InterfaceState.Deleting:
                    InterfaceState = InterfaceState.Deleting;
                    DeleteObjectString = $"Are you sure you want to delete {SelectedEntry.Name} ?";
                    break;
            }
        }

        /// <summary>
        /// Renames the selected entry asynchronously to a new name by renaming
        /// the object in memory and telling checklist service to rename the
        /// entry in the database without blocking UI-thread.
        /// </summary>
        private async Task RenameEntryAsync()
        {
            if (_usingSqlDataBase)
            {
                // Telling service to rename it using background thread.
                await _checklistService.RenameEntryAsync(SelectedEntry.Entry, RenameEntryInput);
            }
            else
            {
                // Renames the selected entry in the full collection to the new name.
                var target = BindingCollection.FirstOrDefault(x => x.ListID == SelectedList.ListID);
                target.Entries.FirstOrDefault(x => x.EntryID == SelectedEntry.EntryID).Name = RenameEntryInput;

                // Saves the newly updated collection locally.
                await _localDataStorageService.StoreObject(BindingCollection);
            }

            // Renaming the entry in memory and setting UI back to normal.
            SelectedEntry.Name = RenameEntryInput;
            InterfaceState = InterfaceState.Default;
        }

        /// <summary>
        /// Removes the selected entry from memory and tells either db service /
        /// local storage service to delete it using background thread.
        /// </summary>
        private async Task DeleteEntryAsync()
        {
            if (_usingSqlDataBase)
            {
                // Requesting the entry be deleted and refreshing the UI once
                // it's been deleted.
                await _checklistService.DeleteEntryAsync(SelectedEntry.Entry);
                await LoadDataIntoMemoryAsync();
            }
            else
            {
                // Removes entry from saved collection.
                var target = BindingCollection.FirstOrDefault(x => x.ListID == SelectedList.ListID);
                target.Entries.Remove(SelectedEntry.Entry);

                // Removes the entry from memory.
                BindingEntryCache.Remove(SelectedEntry);

                // Saves the newly updated collection locally.
                await _localDataStorageService.StoreObject(BindingCollection);
            }

            InterfaceState = InterfaceState.Default;
        }

        /// <summary>
        /// Returns the interface state back to default and removes edit UI.
        /// </summary>
        private void CancelEdit()
        {
            InterfaceState = InterfaceState.Default;
        }

        /// <summary>
        /// Adds a new default entry to the loaded collection, and sends request
        /// to checklist-service / localStorage-service to do the same.
        /// </summary>
        private async Task AddNewDefaultEntryAsync()
        {
            if (_usingSqlDataBase)
            {
                // Tells checklistService to add new default entry to collection and refreshes the view.
                await _checklistService.AddNewDefaultEntryToList(SelectedList);
                await LoadDataIntoMemoryAsync();
            }
            else
            {
                var defaultEntry = new Entry() { Name = "Unnamed Entry!", IsChecked = false, IsDeleted = false, SortOrder = 0 };
                var defaultEntryViewModel = new EntryViewModel(defaultEntry, defaultEntry.EntryID, defaultEntry.Name, defaultEntry.IsChecked, defaultEntry.IsDeleted, defaultEntry.SortOrder, defaultEntry.ListID, _checklistService);
                
                // Adds it to loaded collection and retrieved local collection.
                BindingEntryCache.Add(defaultEntryViewModel);
                var target = BindingCollection.FirstOrDefault(x => x.ListID == SelectedList.ListID);
                target.Entries.Add(defaultEntry);
                
                // Saves the newly updated collection locally.
                await _localDataStorageService.StoreObject(BindingCollection);
            }
        }

        /// <summary>
        /// Marks the entire current visible collection as either checked or
        /// unchecked depending on input parameter.
        /// </summary>
        /// <param name="whatToMarkAs">What to mark IsChecked bool to</param>
        private async Task MarkCurrentCollectionIsChecked(bool whatToMarkAs)
        {
            if (_usingSqlDataBase)
            {
                // TODO : Can optimize this using a linq statement.
                var input = SelectedList.Entries;
                await _checklistService.MarkEntryCollectionIsCheckedFlagAsync(input, whatToMarkAs);
                await LoadDataIntoMemoryAsync();
            }
            else
            {
                // edits the name in json. TODO
                var target = BindingCollection.FirstOrDefault(x => x.ListID == SelectedList.ListID);

                // Marks each element in loaded collection.
                foreach(var entry in BindingEntryCache)
                {
                    entry.IsChecked = whatToMarkAs;
                }

                // Marks each element in saved collection.
                foreach(var entry in target.Entries)
                {
                    entry.IsChecked = whatToMarkAs;
                }

                // Stores the newly updated collection locally.
                await _localDataStorageService.StoreObject(BindingCollection);
                // TODO : Fix updating instead of forcing reload.
                await LoadDataIntoMemoryAsync();
            }
        }

        /// <summary>
        /// Converts a list of 'Entries' into a list of 'EntryViewModels'
        /// </summary>
        /// <param name="inputList">List to convert</param>
        /// <returns>The converted list</returns>
        private ObservableCollection<EntryViewModel> ConvertModelToViewModel(ObservableCollection<Entry> inputList)
        {
            var convertedList = new ObservableCollection<EntryViewModel>();

            if (_usingSqlDataBase)
            {
                foreach (Entry entry in inputList)
                {
                    convertedList.Add(new EntryViewModel(entry, entry.EntryID, entry.Name, entry.IsChecked, entry.IsDeleted, entry.SortOrder, entry.ListID, _checklistService));
                }
            }
            else
            {
                foreach (Entry entry in inputList)
                {
                    convertedList.Add(new EntryViewModel(entry, entry.EntryID, entry.Name, entry.IsChecked, entry.IsDeleted, entry.SortOrder, entry.ListID));
                }
            }

            return convertedList;
        }
        #endregion
    }
}
