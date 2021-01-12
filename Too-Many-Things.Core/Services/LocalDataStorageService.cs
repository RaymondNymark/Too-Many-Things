using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Too_Many_Things.Core.DataAccess.Models;

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
                File.CreateText(_filePath);
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
            }

            return result;
        }
    }
}
