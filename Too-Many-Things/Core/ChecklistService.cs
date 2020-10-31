using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Too_Many_Things.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCore.DbContextScope;

namespace Too_Many_Things.Core
{
    public class ChecklistService : IChecklistService
    {
        /*
         * Despite that it could be argued that splitting up this service into
         * multiple components could improve readability, the scope of this
         * application is low and thus having a single Checklist service class
         * seems appropriate enough.
         */
        private readonly ILogger _logger;
        private readonly IAmbientDbContextLocator _ambientDbContextLocator;
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private TooManyThingsContext DbContext
        {
            get => _ambientDbContextLocator.Get<TooManyThingsContext>() ?? throw new ArgumentNullException("_ambientDbContextLocator");
        }

        public ChecklistService(ILogger logger, IDbContextScopeFactory dbContextScopeFactory, IAmbientDbContextLocator ambientDbContextLocator)
        {
            _logger = logger ?? throw new ArgumentNullException("logger");
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
                dbContextScope.SaveChanges();
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
                    await DbContext.Checklist.AddAsync(checklist);
                    dbContextScope.SaveChanges();
                }
            }
        }
        #endregion


        #region Data validation
        /// <summary>
        /// Validates a checklist's input parameters.
        /// </summary>
        /// <param name="checklist">Checklist to validate</param>
        /// <returns>A boolean value whether or not checklist is valid.</returns>
        private bool ValidateChecklist(Checklist checklist)
        {
            bool output = true;
            // Array of invalid characters.  Due to EF's nature of passing data
            // to the database via LINQ, it should be safe from sketchy
            // characters.  However, this will be left in for future feature.
            char[] invalidCharacters = "[]".ToArray();

            if (checklist.Name.Length > 100)
            {
                output = false;
            }

            if (checklist.Name.IndexOfAny(invalidCharacters) >= 0)
            {
                output = false;
            }

            return output;
        }
        #endregion
    }
}
