using Microsoft.EntityFrameworkCore;
using Too_Many_Things.Core.Services;

namespace Too_Many_Things.Core.DataAccess
{
    public interface IChecklistContextFactory
    {
        ChecklistContext CreateDbContext();
    }

    public class ChecklistContextFactory : IDbContextFactory<ChecklistContext>, IChecklistContextFactory
    {
        private DbContextOptions _options;

        public ChecklistContextFactory()
        {
            _options = new DbContextOptionsBuilder<ChecklistContext>()
                .UseSqlServer(ConnectionStringManager.GetConnectionString())
                .Options;
        }

        public ChecklistContext CreateDbContext()
        {
            return new ChecklistContext(_options);
        }
    }
}
