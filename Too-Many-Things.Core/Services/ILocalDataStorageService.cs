using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Too_Many_Things.Core.DataAccess.Models;

namespace Too_Many_Things.Core.Services
{
    public interface ILocalDataStorageService
    {
        Task<ObservableCollection<List>> RetrieveStoredObjectAsync();
        Task StoreObject(object objectToStore);
    }
}