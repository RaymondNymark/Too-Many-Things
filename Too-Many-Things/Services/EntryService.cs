using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Too_Many_Things.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCore.DbContextScope;
using System.Collections.ObjectModel;

namespace Too_Many_Things.Services
{
    public partial class ChecklistService : IChecklistService
    {
        /// <summary>
        /// Returns an ObservableCollection of entries in a checklist.
        /// </summary>
        /// <param name="checklistID">Checklist ID</param>
        /// <returns></returns>
        public ObservableCollection<Entry> GetEntriesFromChecklist(int checklistID)
        {
            var selected = Get(checklistID);
            var output = selected.Entry;

            return output;
        }

        public void AddNewEntry(Entry entry)
        {
            throw new NotImplementedException();
        }

        public Task AddNewEntryAsync(Entry entry)
        {
            throw new NotImplementedException();
        }

        public void AddNewDefaultEntry()
        {
            throw new NotImplementedException();
        }

        public Task AddNewDefaultEntryAsync()
        {
            throw new NotImplementedException();
        }

        public void DeleteEntry(int entryID)
        {
            throw new NotImplementedException();
        }

        public Task DeleteEntryAsync(int entryID)
        {
            throw new NotImplementedException();
        }

        public void PermenentlyDeletEntry(int entryID)
        {
            throw new NotImplementedException();
        }

        public Task PermenentlyDeleteEntryAsync(int entryID)
        {
            throw new NotImplementedException();
        }

        public void RenameEntry(int entryID, string newName)
        {
            throw new NotImplementedException();
        }

        public Task RenameEntryAsync(int entryID, string newName)
        {
            throw new NotImplementedException();
        }
    }
}
