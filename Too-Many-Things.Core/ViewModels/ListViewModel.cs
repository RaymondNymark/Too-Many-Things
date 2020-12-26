using System.Collections.ObjectModel;
using System.Windows.Media;
using Too_Many_Things.Core.DataAccess.Models;

namespace Too_Many_Things.Core.ViewModels
{
    public class ListViewModel
    {
        public List List { get; set; }
        public int ListID { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public int? SortOrder { get; set; }
        public virtual ObservableCollection<Entry> Entries { get; set; }

        // For entry string and adaptive coloring of it. Black if more than 0,
        // and gray if 0.
        public string EntryString { get; set; }
        public Brush EntryStringBrush { get; set; } = Brushes.DarkSlateGray;

        public ListViewModel(List list, int listID, string name, bool isDeleted, int? sortOrder, ObservableCollection<Entry> entries)
        {
            List = list;
            ListID = listID;
            Name = name;
            IsDeleted = isDeleted;
            SortOrder = sortOrder;
            Entries = entries;

            // As you cannot edit or add entries when ListViewModel is in view,
            // it's pointless to have these properties be reactive to changes.
            EntryString = $"There are {Entries.Count} entries here";
            if (Entries.Count < 3)
            {
                EntryStringBrush = Brushes.DarkGray;
            }
            if (Entries.Count < 1)
            {
                EntryStringBrush = Brushes.LightGray;
            }
        }
    }
}
