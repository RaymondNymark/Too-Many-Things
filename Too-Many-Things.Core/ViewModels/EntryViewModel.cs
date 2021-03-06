﻿using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using System.Threading.Tasks;
using Too_Many_Things.Core.DataAccess.Models;
using Too_Many_Things.Core.Services;

namespace Too_Many_Things.Core.ViewModels
{
    public class EntryViewModel
    {
        public Entry Entry { get; set; }
        public int EntryID { get; set; }
        public string Name { get; set; }

        [Reactive]
        public bool IsChecked { get; set; }
        public bool IsDeleted { get; set; }
        public int SortOrder { get; set; }
        public int ListID { get; set; }
        public List ParentList { get; set; }

        private IChecklistDataService _checklistDataService;
        private ILocalDataStorageService _localDataStorageService;

        public ReactiveCommand<Unit, Unit> CheckboxCommand { get; }

        public EntryViewModel(Entry entry, int entryID, string name, bool isChecked, bool isDeleted, int sortOrder, int listID, IChecklistDataService checklistDataService)
        {
            Entry = entry;
            EntryID = entryID;
            Name = name;
            IsChecked = isChecked;
            IsDeleted = isDeleted;
            SortOrder = sortOrder;
            ListID = listID;
            _checklistDataService = checklistDataService;

            CheckboxCommand = ReactiveCommand.CreateFromTask(() => ToggleIsCheckedAsync());
        }

        public EntryViewModel(Entry entry, int entryID, string name, bool isChecked, bool isDeleted, int sortOrder, int listID, ILocalDataStorageService localDataStorageService, List parentList)
        {
            Entry = entry;
            EntryID = entryID;
            Name = name;
            IsChecked = isChecked;
            IsDeleted = isDeleted;
            SortOrder = sortOrder;
            ListID = listID;
            ParentList = parentList;
            _localDataStorageService = localDataStorageService;

            CheckboxCommand = ReactiveCommand.CreateFromTask(() => ToggleCheckedAsync());
        }

        /// <summary>
        /// Toggles IsChecked flag to opposite of the current entry.
        /// </summary>
        public async Task ToggleIsCheckedAsync()
        {
            await _checklistDataService.ToggleIsCheckedAsync(Entry);
        }

        public async Task ToggleCheckedAsync()
        {
            if (IsChecked)
            {
                IsChecked = false;
            }
            else
            {
                IsChecked = true;
            }

            await _localDataStorageService.ToggleIsChecked(Entry, ParentList);
        }
    }
}
