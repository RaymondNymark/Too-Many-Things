using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Too_Many_Things.Models;
using EntityFramework.DbContextScope.Interfaces;

namespace Too_Many_Things.Core
{
    public class ChecklistRepository : IChecklistRepository
    {
        #region Initialization
        private TooManyThingsContext _dbContext;
        private readonly IAmbientDbContextLocator _contextLocator;

        public ChecklistRepository(TooManyThingsContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        // https://stackoverflow.com/questions/12698793/how-can-i-automatically-filter-out-soft-deleted-entities-with-entity-framework
        // For dealing with soft deletion.

        public Checklist GetChecklistByName(string name)
        {
            using (_dbContext)
            {
                var checklist = _dbContext.Checklist.FirstOrDefault(c => c.Name == name);
                return checklist;
            }
        }

        // TODO: Implement business validation.
        public Checklist GetChecklistByID(int id)
        {
            using (_dbContext)
            {
                var checklist = _dbContext.Checklist.FirstOrDefault(c => c.ChecklistId == id);
                return checklist;
            }
        }

        public DbSet<Checklist> GetChecklistDbSet()
        {
            return _dbContext.Checklist;
        }

        public void CompleteUnitOfWork()
        {
            _dbContext.SaveChanges();
        }
    }
}
