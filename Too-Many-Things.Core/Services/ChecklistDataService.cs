using DynamicData;
using Microsoft.EntityFrameworkCore;
using Splat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Too_Many_Things.Core.DataAccess;
using Too_Many_Things.Core.DataAccess.Models;

namespace Too_Many_Things.Core.Services
{
    public partial class ChecklistDataService : IEnableLogger
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

        // Kind of a messy and inelegant solution. Wonder if there's a better way to do this.
        public List<List> LoadData()
        {
            using (var context = _checklistContextFactory.CreateDbContext())
            {
                var checklists = context.Lists
                    .Include(e => e.Entries)
                    .ToList();

                return checklists;
            }
        }
        
        public ObservableCollection<List> GetLocal()
        {
            using (var context = _checklistContextFactory.CreateDbContext())
            {
                context.Lists.Load();
                context.Entries.Load();

                return context.Lists.Local.ToObservableCollection();
            }
        }

        #region Data-base operations
        public async Task AddDefaultChecklist()
        {
            using (var context = _context)
            {
                this.Log().Info($"Attempting to add defaultChecklist to the database at: {DateTime.UtcNow}.");

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
        public async Task UpdateChecklistNameAsync(List list, string newName)
        {
            using (var context = _context)
            {
                var target = await context.Lists.FindAsync(list.ListID);
                this.Log().Info($"Attempting to change the name of '{target.Name}' to '{newName}' at: {DateTime.UtcNow}.");

                try
                {
                    target.Name = newName;
                    await context.SaveChangesAsync();
                }
                catch(DbUpdateException ex)
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
        public async Task SoftDeleteChecklistAsync(List list)
        {
            using (var context = _context)
            {
                var target = await context.Lists.FindAsync(list.ListID);
                this.Log().Info($"Attempting to soft delete'{target.Name}' at: {DateTime.UtcNow}.");

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
        #endregion
    }
}
