using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Too_Many_Things.Models;

namespace Too_Many_Things.Services
{
    public interface IChecklistService
    {
        Checklist Get(int checklistID);
        Task<Checklist> GetAsync(int checklistID);

        void AddNewChecklist(Checklist checklist);
        Task AddNewChecklistAsync(Checklist checklist);
        void AddNewEntry(Entry entry);
        Task AddNewEntryAsync(Entry entry);

        void AddNewDefaultChecklist();
        Task AddNewDefaultChecklistAsync();
        void AddNewDefaultEntry();
        Task AddNewDefaultEntryAsync();

        void DeleteChecklist(int checklistID);
        Task DeleteChecklistAsync(int checklistID);
        void DeleteEntry(int entryID);
        Task DeleteEntryAsync(int entryID);

        void PermanentlyDeleteChecklist(int checklistID);
        Task PermanentlyDeleteChecklistAsync(int checklistID);
        void PermenentlyDeletEntry(int entryID);
        Task PermenentlyDeleteEntryAsync(int entryID);

        void RenameChecklist(int checklistID, string newName);
        Task RenameChecklistAsync(int checklistID, string newName);
        void RenameEntry(int entryID, string newName);
        Task RenameEntryAsync(int entryID, string newName);

        ObservableCollection<Checklist> GetLocalCollectionSource();
    }
}