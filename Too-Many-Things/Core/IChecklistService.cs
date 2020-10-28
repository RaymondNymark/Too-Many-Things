using System.Collections.Generic;
using System.Threading.Tasks;
using Too_Many_Things.Models;

namespace Too_Many_Things.Core
{
    public interface IChecklistService
    {
        void Get(int checklistID);
        Task GetAsync(int checklistID);


        void CreateChecklist(Checklist checklist);
        Task CreateChecklistAsync(Checklist checklist);


        void CreateDefaultChecklist();
        Task CreateDefaultChecklistAsync();


        void DeleteChecklist(int checklistID);
        Task DeleteChecklistAsync(int checklistID);

        void DeleteChecklist(IList<int> listOfChecklistIDs);
        Task DeleteChecklistAsync(IList<int> listOfChecklistIDs);


        void PermanentlyDeleteChecklist(int checklistID);
        Task PermanentlyDeleteChecklistAsync(int checklistID);


        void RenameChecklist(int checklistID, string newName);
        Task RenameChecklistAsync(int checklistID, string newName);


        void GetLocalView();
        Task GetLocalViewAsync();


        bool ValidateInput(Checklist checklist);
        Task<bool> ValidateInputAsync(Checklist checklist);
    }
}