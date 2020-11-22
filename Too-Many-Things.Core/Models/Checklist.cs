using System;
using System.Collections.ObjectModel;

namespace Too_Many_Things.Core.Models
{
    public partial class Checklist
    {
        public Checklist()
        {
            Entry = new ObservableCollection<Entry>();
        }

        public int ChecklistId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public int? SortOrder { get; set; }

        public virtual ObservableCollection<Entry> Entry { get; set; }
    }
}
