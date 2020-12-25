using System;
using System.Collections.Generic;
using System.Text;

namespace Too_Many_Things.Core.ViewModels
{
    public class EntryViewModel
    {
        public int EntryID { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public bool IsDeleted { get; set; }
        public int SortOrder { get; set; }
        public int ListID { get; set; }

        public EntryViewModel(int entryID, string name, bool isChecked, bool isDeleted, int sortOrder, int listID)
        {
            EntryID = entryID;
            Name = name;
            IsChecked = isChecked;
            IsDeleted = isDeleted;
            SortOrder = sortOrder;
            ListID = listID;
        }
    }
}
