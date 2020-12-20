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
    }
}
