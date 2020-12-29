using System.Collections.Generic;
using System.Threading.Tasks;
using Too_Many_Things.Core.DataAccess.Models;

namespace Too_Many_Things.Core.Services
{
    public interface IChecklistDataService
    {
        Task AddDefaultChecklist();
        Task AddNewDefaultEntryToList(List listToAddEntryTo);
        Task DeleteEntryAsync(Entry entryToDelete);
        Task<List<List>> LoadDataAsync();
        Task<List<Entry>> LoadEntryDataAsync(List checklist);
        Task RenameEntryAsync(Entry entryToRename, string newName);
        Task SoftDeleteChecklistAsync(List checklistToSoftDelete);
        Task ToggleIsCheckedAsync(Entry entryToMarkAsChecked);
        Task UpdateChecklistNameAsync(List checklistToRename, string newName);
    }
}