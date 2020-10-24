using Microsoft.EntityFrameworkCore;
using Too_Many_Things.Models;

namespace Too_Many_Things.Core
{
    public interface IChecklistRepository
    {
        Checklist GetChecklistByID(int id);
        Checklist GetChecklistByName(string name);
        DbSet<Checklist> GetChecklistDbSet();
        public void CompleteUnitOfWork();
    }
}