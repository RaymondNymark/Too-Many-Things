using System;
using System.Collections.Generic;
using System.Text;

namespace Too_Many_Things.Core.Models
{
    public partial class Checklist
    {
        // TODO : Get rid of this horribleness without modifying model and fix
        // template problem.  Also, this absolutely will not update reactively.
        public int EntryCount
        {
            get => this.Entry.Count;
        }

        public string EntryCountString
        {
            get => $"There are {EntryCount} entries here.";
        }
    }
}
