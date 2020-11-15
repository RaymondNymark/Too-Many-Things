using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Too_Many_Things.Models;

namespace Too_Many_Things.Services
{
    public interface IChecklistService
    {
        Checklist Get(int checklistID);
        ValueTask<Checklist> GetAsync(int checklistID);


        void AddNewChecklist(Checklist checklist);
        Task AddNewChecklistAsync(Checklist checklist);


        void AddNewDefaultChecklist();
        Task AddNewDefaultChecklistAsync();


        void DeleteChecklist(int checklistID);
        Task DeleteChecklistAsync(int checklistID);

        void DeleteChecklist(IList<int> listOfChecklistIDs);
        Task DeleteChecklistAsync(IList<int> listOfChecklistIDs);


        void PermanentlyDeleteChecklist(int checklistID);
        Task PermanentlyDeleteChecklistAsync(int checklistID);


        void RenameChecklist(int checklistID, string newName);
        Task RenameChecklistAsync(int checklistID, string newName);

        ObservableCollection<Checklist> GetLocalCollectionSource();
    }
}