using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Too_Many_Things.Core.DataAccess.Models;
using Too_Many_Things.Core.ViewModels;

namespace Too_Many_Things.Core.Services
{
    public interface ILocalDataStorageService
    {
        Task<ObservableCollection<List>> RetrieveStoredObjectAsync();
        Task StoreObject(ObservableCollection<List> objectToStore);

        Task ConvertAndStoreListCollectionAsync(ObservableCollection<ListViewModel> listToStore);
    }
}