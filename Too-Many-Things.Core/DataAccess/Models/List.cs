using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Too_Many_Things.Core.DataAccess.Models
{
    public partial class List
    {
        public int ListID { get; set; }

        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [Required]
        public bool IsDeleted { get; set; }

        public int? SortOrder { get; set; }

        public virtual ObservableCollection<Entry> Entries { get; set; } = new ObservableCollection<Entry>();
    }
}
