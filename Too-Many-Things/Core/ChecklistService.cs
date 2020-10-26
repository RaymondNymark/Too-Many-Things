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

        #region Traditional 'repostory' services.
        private Checklist Get(int checklistID)
        {
            return DbContext.Checklist.Find(checklistID);
        }
        private ValueTask<Checklist> GetAsync(int checklistID)
        {
            return DbContext.Checklist.FindAsync(checklistID);
        }

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


        #region Updated Services
        public void TestService(int checklistID)
        {
            using (var dbContextScope = _dbContextScopeFactory.Create())
            {
                var checklist = Get(checklistID);
                // [...}
                dbContextScope.SaveChanges();
            }
        }
        #endregion


        #region Data validation
        #endregion
    }
}
