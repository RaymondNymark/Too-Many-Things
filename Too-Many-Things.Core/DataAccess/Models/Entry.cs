using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Too_Many_Things.Core.DataAccess.Models
{
    public partial class Entry
    {
        public int EntryID { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        public bool IsChecked { get; set; }
        public bool IsDeleted { get; set; }
        public int SortOrder { get; set; }

        [Required]
        public int ListID { get; set; }
        [Required]
        public virtual List List { get; set; }
    }
}
