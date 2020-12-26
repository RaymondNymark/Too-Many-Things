using System;
using System.Collections.Generic;
using System.Text;
using Too_Many_Things.Core.DataAccess.Models;

namespace Too_Many_Things.Core.ViewModels
{
    public class EntryViewModel
    {
        public Entry Entry { get; set; }
        public int EntryID { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public bool IsDeleted { get; set; }
        public int SortOrder { get; set; }
        public int ListID { get; set; }

        public EntryViewModel(Entry entry, int entryID, string name, bool isChecked, bool isDeleted, int sortOrder, int listID)
        {
            Entry = entry;
            EntryID = entryID;
            Name = name;
            IsChecked = isChecked;
            IsDeleted = isDeleted;
            SortOrder = sortOrder;
            ListID = listID;
        }
    }
}
