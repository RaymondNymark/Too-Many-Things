using EntityFramework.DbContextScope.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Too_Many_Things.Models;
using System.Linq;
using System.Threading.Tasks;

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

        //private readonly IDbContextScopeFactory _dbContextScopeFactory;
        //private readonly IAmbientDbContextLocator _ambientDbContextLocator;

        //private TooManyThingsContext _dbContext
        //{
        //    get
        //    {
        //        var dbContext = _ambientDbContextLocator.Get<TooManyThingsContext>();
        //    }
        //}

        //public ChecklistService(IDbContextScopeFactory dbContextScopeFactory, IAmbientDbContextLocator ambientDbContextLocator)
        //{
        //    if (dbContextScopeFactory == null) throw new ArgumentNullException("dbContextScopeFactory");
        //    if (ambientDbContextLocator == null) throw new ArgumentNullException("ambientDbContextLocator");
        //    _dbContextScopeFactory = dbContextScopeFactory;
        //    _ambientDbContextLocator = ambientDbContextLocator;
        //}
        private readonly TooManyThingsContext _dbContext;
        private readonly ILogger _logger;

        public ChecklistService(TooManyThingsContext dbContext, ILogger logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException("dbContext");
            _logger = logger ?? throw new ArgumentNullException("logger");
        }

        #region Internal Read operations
        private Checklist Get(int checklistID) => _dbContext.Set<Checklist>().FirstOrDefault(i => i.ChecklistId == checklistID);
        private Checklist Get(string checklistName) => _dbContext.Set<Checklist>().FirstOrDefault(i => i.Name == checklistName);
        //private Task<Checklist> GetAsync(int checklistID) => _dbContext.Set<Checklist>().FindAsync(checklistID);
        #endregion



        #region Services

        // TODO : Check data validation for all of these.

        public void CreateChecklist(Checklist checklist)
        {
            _dbContext.Add(checklist);
        }
        public async Task CreateChecklistAsync(Checklist checklist)
        {
            await _dbContext.AddAsync(checklist);
        }

        public void RemoveChecklist(int checklistID)
        {
            var checklist = Get(checklistID);
            checklist.IsDeleted = true;
        }
        public async Task RemoveChecklistAsync(int checklistID)
        {
            throw new NotImplementedException();
        }

        public void RenameChecklist(int checklistID, string newName)
        {
            var checklist = Get(checklistID);
            checklist.Name = newName;
        }

        public async Task RenameChecklistAsync(int checklistID, string newName)
        {
            throw new NotImplementedException();
        }

        // TODO!! : Implement methods to re-order items.!!

        #endregion

        #region Data validation
        #endregion
    }
}
