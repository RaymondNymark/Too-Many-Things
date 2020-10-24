using EntityFramework.DbContextScope.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Too_Many_Things.Models;

namespace Too_Many_Things.Core
{
    public class ChecklistService
    {
        /*
         * Despite that it could be argued that splitting up this service into
         * multiple components could improve readability, the scope of this
         * application is low and thus having a single Checklist service class
         * seems appropriate enough.
         */

        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IAmbientDbContextLocator _ambientDbContextLocator;

        //private TooManyThingsContext _dbContext
        //{
        //    get
        //    {
        //        var dbContext = _ambientDbContextLocator.Get<TooManyThingsContext>();
        //    }
        //}

        public ChecklistService(IDbContextScopeFactory dbContextScopeFactory, IAmbientDbContextLocator ambientDbContextLocator)
        {
            if (dbContextScopeFactory == null) throw new ArgumentNullException("dbContextScopeFactory");
            if (ambientDbContextLocator == null) throw new ArgumentNullException("ambientDbContextLocator");
            _dbContextScopeFactory = dbContextScopeFactory;
            _ambientDbContextLocator = ambientDbContextLocator;
        }

        #region Internal Read operations
        private Checklist Get(int ChecklistID)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var dbcontext = scope.DbContexts.Get<TooManyThingsContext>();
            }
        }
        #endregion

        #region Services
        public void MarkChecklistAsDeleted(int checklistID)
        {
            var selection = _checklistRepository.GetChecklistByID(checklistID);
            selection.IsDeleted = true;
            _checklistRepository.CompleteUnitOfWork();
        }

        public void RenameChecklist(Checklist selectedChecklist, string newName)
        {
            selectedChecklist.Name = newName;
            _checklistRepository.CompleteUnitOfWork();
        }

        #endregion
    }
}
