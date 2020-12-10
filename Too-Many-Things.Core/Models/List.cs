using System;
using System.Collections.Generic;

namespace Too_Many_Things.Core.Models
{
    public partial class List
    {
        public int ListID { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public int? SortOrder { get; set; }

        public virtual ICollection<Entry> Entries { get; set; }
    }
}
