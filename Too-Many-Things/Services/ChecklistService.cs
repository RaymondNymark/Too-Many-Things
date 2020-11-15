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
using System.Windows.Data;
using static Too_Many_Things.Enums.Enums;

namespace Too_Many_Things.Services
{
    public partial class ChecklistService : IChecklistService
    {
        /*
         * The full service class is split into two files. This class will hold
         * onto everything related to the outer checklists, whilst
         * EntryService.cs will hold onto everything related to the actual
         * entries in Checklists.
         */
        private readonly IAmbientDbContextLocator _ambientDbContextLocator;
        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        // Read-only property that returns a readonly-DbContext.
        private TooManyThingsContext _dbContext
        {
            get
            {
                // This is causing a memory leak.
                // TODO : Dispose of this
                var _dbContext = _dbContextScopeFactory.Create().DbContexts.Get<TooManyThingsContext>();

                if (_dbContext == null)
                {
                    throw new ArgumentNullException("_dbContext");
                }

                return _dbContext;
            }
        }

        public ChecklistService(IDbContextScopeFactory dbContextScopeFactory, IAmbientDbContextLocator ambientDbContextLocator)
        {
            _dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException("dbContextScopeFactory");
            _ambientDbContextLocator = ambientDbContextLocator ?? throw new ArgumentNullException("ambientDbContextLocator");
        }

        #region Traditional 'repository' services.
        /// <summary>
        /// Finds checklist in the database by checklistID.
        /// </summary>
        /// <returns>Returns checklist.  If not found, returns null.</returns>
        public Checklist Get(int checklistID)
        {
            var target = _dbContext.Checklist.Find(checklistID);
            return target;
        }
        public async Task<Checklist> GetAsync(int checklistID)
        {
            var target = await _dbContext.Checklist.FindAsync(checklistID);
            return target;
        }
        #endregion

        #region Checklist Related Services
        /// <summary>
        /// Adds a checklist to the Checklist database. Does nothing if
        /// checklist fails to validate.
        /// </summary>
        /// <param name="checklist">Checklist to add.</param>
        public void AddNewChecklist(Checklist checklist)
        {
            if (ValidateChecklist(checklist))
            {
                using (var dbContextScope = _dbContextScopeFactory.Create())
                {
                    _dbContext.Checklist.Add(checklist);
                    dbContextScope.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Adds a checklist to the checklist database asynchronously. Does
        /// nothing if checklist fails to validate.
        /// </summary>
        /// <param name="checklist">Checklist to add.</param>
        public async Task AddNewChecklistAsync(Checklist checklist)
        {
            if (ValidateChecklist(checklist))
            {
                using (var dbContextScope = _dbContextScopeFactory.Create())
                {
                    _dbContext.Checklist.Add(checklist);
                    await dbContextScope.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Creates a new default checklist and adds it to the database.
        /// </summary>
        public void AddNewDefaultChecklist()
        {
            var defaultChecklist = new Checklist { Name = "New checklist" };
            AddNewChecklist(defaultChecklist);
        }

        /// <summary>
        /// Creates a new default checklist and adds it to the database Asynchronously. 
        /// </summary>
        public async Task AddNewDefaultChecklistAsync()
        {
            var defaultChecklist = new Checklist { Name = "New checklist" };
            await AddNewChecklistAsync(defaultChecklist);
        }

        /// <summary>
        /// Marks a checklist as deleted by checklistID. Also known as
        /// soft-deletion.
        /// </summary>
        /// <param name="checklistID">Checklist to mark as deleted.</param>
        public void DeleteChecklist(int checklistID)
        {
            var target = Get(checklistID);

            if (target != null)
            {
                using (var dbContextScope = _dbContextScopeFactory.Create())
                {
                    target.IsDeleted = true;
                    dbContextScope.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Marks a checklist as deleted by checklistID asynchronously. Also known as
        /// soft-deletion.
        /// </summary>
        /// <param name="checklistID">Checklist to mark as deleted.</param>
        public async Task DeleteChecklistAsync(int checklistID)
        {
            var target = await GetAsync(checklistID);
            
            if (target != null)
            {
                using (var dbContextScope = _dbContextScopeFactory.Create())
                {
                    target.IsDeleted = true;
                    await dbContextScope.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Permanently deletes a checklist from database. Also known as
        /// hard-deletion.
        /// </summary>
        /// <param name="checklistID">ChecklistID to remove.</param>
        public void PermanentlyDeleteChecklist(int checklistID)
        {
            var target = Get(checklistID);

            if (target != null)
            {
                using (var dbContextScope = _dbContextScopeFactory.Create())
                {
                    _dbContext.Remove(checklistID);
                    dbContextScope.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Permanently deletes a checklist from database asynchronously. Also
        /// known as hard-deletion.
        /// </summary>
        /// <param name="checklistID">ChecklistID to remove.</param>
        public async Task PermanentlyDeleteChecklistAsync(int checklistID)
        {
            var target = GetAsync(checklistID);

            if (target != null)
            {
                using (var dbContextScope = _dbContextScopeFactory.Create())
                {
                    _dbContext.Remove(checklistID);
                    await dbContextScope.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Renames a checklist to a new name.
        /// </summary>
        /// <param name="checklistID">ChecklistID to rename.</param>
        /// <param name="newName">New name for the checklist.</param>
        public void RenameChecklist(int checklistID, string newName)
        {
            if (ValidateName(newName))
            {
                using (var dbContextScope = _dbContextScopeFactory.Create())
                {
                    var target = Get(checklistID);
                    target.Name = newName;

                    dbContextScope.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Renames a checklist to a new name asynchronously.
        /// </summary>
        /// <param name="checklistID">ChecklistID to rename.</param>
        /// <param name="newName">New name for the checklist.</param>
        public async Task RenameChecklistAsync(int checklistID, string newName)
        {
            if (ValidateName(newName))
            {
                using (var dbContextScope = _dbContextScopeFactory.Create())
                {
                    var target = await GetAsync(checklistID);
                    target.Name = newName;

                    await dbContextScope.SaveChangesAsync();
                }
            }
        }
        #endregion


        #region Data-binding Source related things
        /// <summary>
        /// Automatically populates the local set with data and returns an
        /// observable collection.  This collection will stay in sync as
        /// entities are added or removed from the context it belongs to. Useful
        /// for data binding!
        /// </summary>
        public ObservableCollection<Checklist> GetLocalCollectionSource()
        {
            _dbContext.Checklist.Load<Checklist>();
            var localCollection = _dbContext.Checklist.Local.ToObservableCollection();

            return localCollection;
        } 

        /// <summary>
        /// Gets back a sorted collection to use for data binding.
        /// </summary>
        /// <param name="sortOrder">How to sort the collection. Possible sort
        /// orders are: AscendingOrder, DescendingOrder, ByAscendingValue,
        /// ByDescendingValue. Pass nothing in for default sort.</param>
        /// <returns></returns>
        public ObservableCollection<Checklist> GetSortedCollection(SortOrder sortOrder = SortOrder.Default)
        {
            var unsortedCollection = GetLocalCollectionSource();
            var sortedCollection = new ObservableCollection<Checklist>();

            switch (sortOrder)
            {
                case SortOrder.Default:
                    sortedCollection.OrderBy(i => i.SortOrder);
                    break;
                case SortOrder.AscendingOrder:
                    sortedCollection.OrderBy(i => i.Name);
                    break;
                case SortOrder.DescendingOrder:
                    sortedCollection.OrderByDescending(i => i.Name);
                    break;
            }

            return sortedCollection;
        }

        #endregion

        #region Data validation
        /// <summary>
        /// Validates a checklist's input parameters.
        /// </summary>
        /// <param name="checklist">Checklist to validate</param>
        /// <returns>A boolean value whether or not checklist is valid.</returns>
        public static bool ValidateChecklist(Checklist checklist)
        {
            bool output = true;
            // Array of invalid characters.  Due to EF's nature of passing data
            // to the database via LINQ, it should be safe from sketchy
            // characters.  However, this will be left in for future feature.
            char[] invalidCharacters = "[]".ToArray();

            if (checklist.Name.Length > 50)
            {
                output = false;
            }

            if (checklist.Name.IndexOfAny(invalidCharacters) >= 0)
            {
                output = false;
            }

            if (string.IsNullOrWhiteSpace(checklist.Name))
            {
                output = false;
            }

            return output;
        }

        public static bool ValidateName(string name)
        {
            bool output = true;
            char[] invalidCharacters = "[]".ToArray();

            if (name.Length > 50)
            {
                output = false;
            }

            if (name.IndexOfAny(invalidCharacters) >= 0)
            {
                output = false;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                output = false;
            }

            return output;
        }
        #endregion
    }
}
