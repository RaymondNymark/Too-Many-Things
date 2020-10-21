using System;
using System.Collections.ObjectModel;

namespace Too_Many_Things.Models
{
    public partial class Entry
    {
        public int EntryId { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public bool IsDeleted { get; set; }
        public int ChecklistId { get; set; }

        public virtual Checklist Checklist { get; set; }
    }
}
