using Microsoft.EntityFrameworkCore;
using Splat;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Too_Many_Things.Core.DataAccess;
using Too_Many_Things.Core.DataAccess.Models;

namespace Too_Many_Things.Core.Services
{
    public partial class ChecklistDataService : IEnableLogger, IChecklistDataService
    {
        private IChecklistContextFactory _checklistContextFactory;
        private ChecklistContext _context
        {
            get => _checklistContextFactory.CreateDbContext();
        }

        // Connection string has to be initialized before this class is created.
        public ChecklistDataService(IChecklistContextFactory checklistContextFactory = null)
        {
            _checklistContextFactory = checklistContextFactory ?? Locator.Current.GetService<IChecklistContextFactory>();
        }

        /// <summary>
        /// Loads all of the data that is not marked with IsDeleted flag asynchronously.
        /// </summary>
        /// <returns>List of the data when awaited</returns>
        public async Task<List<List>> LoadDataAsync()
        {
            using (var context = _context)
            {
                var result = await context.Lists
                    .Include(e => e.Entries)
                    .Where(m => EF.Property<bool>(m, "IsDeleted") != true)
                    .ToListAsync();

                return result;
            }
        }

        /// <summary>
        /// Loads all of the entries inside a specific checklist that are not marked as deleted.
        /// </summary>
        /// <returns>A list of entries that belong to a checklist</returns>
        public async Task<List<Entry>> LoadEntryDataAsync(List checklist)
        {
            using (var context = _context)
            {
                var target = await context.Lists.FindAsync(checklist.ListID);
                await context.Entries.LoadAsync();

                var result = target.Entries.ToList();

                return result;
            }
        }
        #region Data-base operations
        /// <summary>
        /// Adds a default checklist named "Unnamed Checklist!" to the database.
        /// </summary>
        public async Task AddDefaultChecklistAsync()
        {
            using (var context = _context)
            {
                this.Log().Info($"Attempting to add defaultChecklist to the database.");

                var defaultChecklist = new List { Name = "Unnamed Checklist!", IsDeleted = false, SortOrder = 0 };
                await context.AddAsync(defaultChecklist);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Updates the name of a checklist asynchronously.
        /// </summary>
        /// <param name="list">List to update</param>
        /// <param name="newName">new name to update to</param>
        public async Task UpdateChecklistNameAsync(List checklistToRename, string newName)
        {
            using (var context = _context)
            {
                var target = await context.Lists.FindAsync(checklistToRename.ListID);
                this.Log().Info($"Attempting to change the name of '{target.Name}' to '{newName}'.");

                try
                {
                    target.Name = newName;
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    this.Log().Error(ex, "Exception encountered trying to change name of a checklist");
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Marks a checklist as deleted, also known as soft-deleting.  Entities
        /// that are soft-deleted are not retrieved when data is loaded from the
        /// database.
        /// </summary>
        /// <param name="list">Checklist to mark as deleted</param>
        public async Task SoftDeleteChecklistAsync(List checklistToSoftDelete)
        {
            using (var context = _context)
            {
                var target = await context.Lists.FindAsync(checklistToSoftDelete.ListID);
                this.Log().Info($"Attempting to soft delete'{target.Name}'.");

                try
                {
                    target.IsDeleted = true;
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    this.Log().Error(ex, "Exception encountered trying to soft delete a checklist");
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Toggles the IsChecked value of an entry to the opposite value.
        /// </summary>
        /// <param name="entry">Entry to toggle IsChecked flag on</param>
        public async Task ToggleIsCheckedAsync(Entry entryToMarkAsChecked)
        {
            using (var context = _context)
            {
                var target = await context.Entries.FindAsync(entryToMarkAsChecked.EntryID);
                bool newFlag;

                switch (target.IsChecked)
                {
                    case true:
                        newFlag = false;
                        break;
                    case false:
                        newFlag = true;
                        break;
                }

                try
                {
                    target.IsChecked = newFlag;
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    this.Log().Error(ex, $"Exception encountered trying to mark 'IsChecked' of {entryToMarkAsChecked.Name}");
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Deletes an entry permanently from the database.
        /// </summary>
        public async Task DeleteEntryAsync(Entry entryToDelete)
        {
            using (var context = _context)
            {
                try
                {
                    var target = await context.Entries.FindAsync(entryToDelete.EntryID);
                    this.Log().Info($"Attempting to delete {target.Name} from the database.");
                    context.Entries.Remove(target);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    this.Log().Error(ex, $"Exception encountered trying to delete {entryToDelete.Name}.");
                    throw ex;
                }
            }
        }

        public async Task RenameEntryAsync(Entry entryToRename, string newName)
        {
            using (var context = _context)
            {
                try
                {
                    var target = await context.Entries.FindAsync(entryToRename.EntryID);
                    this.Log().Info($"Attempting to rename {entryToRename.Name} to {newName}.");
                    target.Name = newName;
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    this.Log().Error(ex, $"Exception encountered trying to delete {entryToRename.Name}.");
                }
            }
        }

        /// <summary>
        /// Adds a new default entry to a checklist
        /// </summary>
        /// <param name="listToAddEntryTo">List to add an entry to</param>
        public async Task AddNewDefaultEntryToList(List listToAddEntryTo)
        {
            using (var context = _context)
            {
                try
                {
                    var target = await context.Lists.FindAsync(listToAddEntryTo.ListID);
                    var defaultEntry = new Entry() { Name = "Unnamed Entry!", IsChecked = false, IsDeleted = false, SortOrder = 0 };

                    this.Log().Info($"Attempting to add a new default entry in {listToAddEntryTo.Name}.");
                    target.Entries.Add(defaultEntry);

                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    this.Log().Error(ex, $"Exception encountered trying to add default entry to {listToAddEntryTo.Name}");
                }
            }
        }

        /// <summary>
        /// Marks every item's IsChecked bool in a collection of entries to specified value.   
        /// </summary>
        /// <param name="whatToMarkAs">What to marks IsChecked as</param>
        public async Task MarkEntryCollectionIsCheckedFlagAsync(ObservableCollection<Entry> collectionOfEntries, bool whatToMarkAs)
        {
            var newFlag = whatToMarkAs;

            using (var context = _context)
            {
                try
                {
                    this.Log().Info($"Attempting to set the IsChecked flag of every entity in {collectionOfEntries} to {whatToMarkAs}.");
                    foreach (Entry entry in collectionOfEntries)
                    {
                        var target = await context.Entries.FindAsync(entry.EntryID);
                        target.IsChecked = newFlag;
                    }

                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    this.Log().Error(ex, $"Exception encountered trying to mark IsChecked of {collectionOfEntries} to {whatToMarkAs}. Exception: {ex}.");
                }
            }
        }
        #endregion
    }
}
