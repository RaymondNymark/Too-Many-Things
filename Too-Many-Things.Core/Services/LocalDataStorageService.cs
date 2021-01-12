using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Too_Many_Things.Core.DataAccess.Models;
using Too_Many_Things.Core.ViewModels;

namespace Too_Many_Things.Core.Services
{
    public class LocalDataStorageService : ILocalDataStorageService
    {
        private string _path { get; set; }
        private string _filePath { get; set; }

        public LocalDataStorageService()
        {
            Initialize();
        }

        /// <summary>
        /// Starts the process of creating the means of storing data locally on
        /// the user's computer as opposed to an online database.
        /// </summary>
        private void Initialize()
        {
            // Check if Too-Many-Things folder exists in roaming app data and
            // create if it does not.
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _path = appDataPath + @"\Too-Many-Things";
            _filePath = _path + @"\localChecklistData.json";

            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            // Check if localChecklistData.json file exists and creates if it doesn't.
            if (!File.Exists(_filePath))
            {
                var newFile = File.CreateText(_filePath);
                newFile.Close();
            }
        }

        /// <summary>
        /// Stores an object into the localChecklistData.json file to be able to
        /// be retrieved between sessions.
        /// </summary>
        /// <param name="objectToStore">Object to store</param>
        public async Task StoreObject(object objectToStore)
        {
            using (StreamWriter file = new StreamWriter(_filePath))
            {
                // Serializing the object into localChecklistData.json.
                await file.WriteLineAsync(JsonConvert.SerializeObject(objectToStore, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
            }
        }

        /// <summary>
        /// Retrieves the stored object in localChecklistData.json. Returns null if it's empty.
        /// </summary>
        /// <returns>The stored object</returns>
        public async Task<ObservableCollection<List>> RetrieveStoredObjectAsync()
        {
            ObservableCollection<List> result;

            using (StreamReader file = new StreamReader(_filePath))
            {
                // De-serializing the data back into an observableCollection.
                result = JsonConvert.DeserializeObject<ObservableCollection<List>>(await file.ReadToEndAsync());

                if (result == null)
                {
                    // Returns empty ObservableCollection rather than null if it's empty.
                    result = new ObservableCollection<List>();
                }
            }

            return result;
        }

        /// <summary>
        /// Converts and stores a ListViewModel list into ObservableCollection T where T is List."
        /// </summary>
        /// <param name="listToStore">List to convert and save.</param>
        public async Task ConvertAndStoreListCollectionAsync(ObservableCollection<ListViewModel> listToStore)
        {
            // Converts ListViewModel back to ObservableCollection.
            ObservableCollection<List> collection = new ObservableCollection<List>();

            foreach(var list in listToStore)
            {
                collection.Add(list.List);
            }

            await StoreObject(collection);
        }
    }
}
