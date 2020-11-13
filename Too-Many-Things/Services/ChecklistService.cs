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
    public class ChecklistService : IChecklistService
    {
        /*
         * Despite that it could be argued that splitting up this service into
         * multiple components could improve readability, the scope of this
         * application is low and thus having a single Checklist service class
         * seems appropriate enough.
         */
        private readonly IAmbientDbContextLocator _ambientDbContextLocator;
        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        // Read-only property that returns a readonly-DbContext.
        private TooManyThingsContext DbContext
        {
            get
            {
                using (var scope = _dbContextScopeFactory.Create())
                {
                    // This may cause a memory leak.
                    var _dbContext = scope.DbContexts.Get<TooManyThingsContext>();

                    if (_dbContext == null)
                    {
                        throw new ArgumentNullException("_dbContext");
                    }

                    return _dbContext;
                }
            }
        }

        public ChecklistService(IDbContextScopeFactory dbContextScopeFactory, IAmbientDbContextLocator ambientDbContextLocator)
        {
            _dbContextScopeFactory = dbContextScopeFactory ?? throw new ArgumentNullException("dbContextScopeFactory");
            _ambientDbContextLocator = ambientDbContextLocator ?? throw new ArgumentNullException("ambientDbContextLocator");
        }

        #region Traditional 'repository' services.
        public Checklist Get(int checklistID)
        {
            return DbContext.Checklist.Find(checklistID);
        }
        public ValueTask<Checklist> GetAsync(int checklistID)
        {
            return DbContext.Checklist.FindAsync(checklistID);
        }
        #endregion

        //#region Services

        //// TODO : Check data validation for all of these.

        //public void CreateChecklist(Checklist checklist)
        //{
        //    //_dbContext.Add(checklist);
        //}
        //public async Task CreateChecklistAsync(Checklist checklist)
        //{
        //    //await _dbContext.AddAsync(checklist);
        //}

        //public void RemoveChecklist(int checklistID)
        //{
        //    //var checklist = Get(checklistID);
        //    //checklist.IsDeleted = true;
        //}
        //public async Task RemoveChecklistAsync(int checklistID)
        //{
        //    throw new NotImplementedException();
        //}

        //public void RenameChecklist(int checklistID, string newName)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task RenameChecklistAsync(int checklistID, string newName)
        //{
        //    throw new NotImplementedException();
        //}

        //// TODO!! : Implement methods to re-order items.!!
        //#endregion


        #region Updated Services
        // Template:
        public void TestService(int checklistID)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var checklist = Get(checklistID);
                // [...}
                try
                {
                    dbContextScope.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Adds a checklist to the Checklist database.
        /// </summary>
        /// <param name="checklist">Checklist to add</param>
        public void CreateChecklist(Checklist checklist)
        {
            if (ValidateChecklist(checklist))
            {
                using (var dbContextScope = _dbContextScopeFactory.Create())
                {
                    DbContext.Checklist.Add(checklist);
                    dbContextScope.SaveChanges();
                }
            }
        }

        public async Task CreateChecklistAsync(Checklist checklist)
        {
            if (ValidateChecklist(checklist))
            {
                using (var dbContextScope = _dbContextScopeFactory.Create())
                {
                    DbContext.Checklist.AddAsync(checklist);
                    await dbContextScope.SaveChangesAsync();
                }
            }
        }

        /// <summary>
        /// Creates a new default checklist and adds it to the database.
        /// </summary>
        public void CreateDefaultChecklist()
        {
            var defaultChecklist = new Checklist { Name = "New checklist" };
            CreateChecklist(defaultChecklist);
        }

        /// <summary>
        /// Creates a new default checklist and adds it to the database Asynchronously. 
        /// </summary>
        /// <returns></returns>
        public async Task CreateDefaultChecklistAsync()
        {
            var defaultChecklist = new Checklist { Name = "New checklist" };
            await CreateChecklistAsync(defaultChecklist);
        }

        /// <summary>
        /// Marks a checklist as deleted by checklistID. Also known as
        /// soft-deletion.
        /// </summary>
        /// <param name="checklistID">Checklist to mark as deleted.</param>
        public void DeleteChecklist(int checklistID)
        {
            var target = Get(checklistID);

            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                target.IsDeleted = true;
                dbContextScope.SaveChanges();
            }
        }

        public Task DeleteChecklistAsync(int checklistID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Marks a list of checklist IDs as deleted. Also known as
        /// soft-deletion.
        /// </summary>
        /// <param name="listOfChecklistIDs">List of checklistIDs</param>
        public void DeleteChecklist(IList<int> listOfChecklistIDs)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                foreach (var id in listOfChecklistIDs)
                {
                    var target = Get(id);
                    target.IsDeleted = true;
                }
                dbContextScope.SaveChanges();
            }
        }

        public Task DeleteChecklistAsync(IList<int> listOfChecklistIDs)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Permanently deletes a checklist from database. Also known as
        /// hard-deletion.
        /// </summary>
        /// <param name="checklistID">ChecklistID to remove.</param>
        public void PermanentlyDeleteChecklist(int checklistID)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var target = Get(checklistID);
                DbContext.Remove(target);

                dbContextScope.SaveChanges();
            }
        }

        public Task PermanentlyDeleteChecklistAsync(int checklistID)
        {
            throw new NotImplementedException();
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

        public Task RenameChecklistAsync(int checklistID, string newName)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Checklist> GetLocalView()
        {
            return DbContext.Checklist.Local.ToObservableCollection();
        }

        /// <summary>
        /// Automatically populates the local set with data and returns an
        /// observable collection.  This collection will stay in sync as
        /// entities are added or removed from the context it belongs to. Useful
        /// for data binding!
        /// </summary>
        public ObservableCollection<Checklist> GetLocalCollectionSource()
        {
            DbContext.Checklist.Load<Checklist>();
            var localCollection = DbContext.Checklist.Local.ToObservableCollection();

            return localCollection;
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
            return output;
        }
        #endregion
    }
}
