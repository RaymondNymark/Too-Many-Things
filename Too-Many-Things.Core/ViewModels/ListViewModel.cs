using System.Collections.ObjectModel;
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

        public ListViewModel(List list, int listID, string name, bool isDeleted, int? sortOrder, ObservableCollection<Entry> entries)
        {
            List = list;
            ListID = listID;
            Name = name;
            IsDeleted = isDeleted;
            SortOrder = sortOrder;
            Entries = entries;
        }
    }
}
