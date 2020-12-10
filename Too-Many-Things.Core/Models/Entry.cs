﻿using System;
using System.Collections.Generic;

namespace Too_Many_Things.Core.Models
{
    public partial class Entry
    {
        public int EntryID { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        public bool IsDeleted { get; set; }
        public int? SortOrder { get; set; }

        public virtual List List { get; set; }
    }
}
