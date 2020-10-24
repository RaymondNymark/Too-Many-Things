using System.Threading.Tasks;
using Too_Many_Things.Models;

namespace Too_Many_Things.Core
{
    public interface IChecklistService
    {
        void CreateChecklist(Checklist checklist);
        Task CreateChecklistAsync(Checklist checklist);
        void RemoveChecklist(int checklistID);
        Task RemoveChecklistAsync(int checklistID);
        void RenameChecklist(int checklistID, string newName);
        Task RenameChecklistAsync(int checklistID, string newName);
    }
}