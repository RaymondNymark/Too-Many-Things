using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Too_Many_Things.Core.Services
{
    /// <summary>
    /// This class saves and retrieves ConnectionString saved on a txt file.
    /// Because this is not a public connectionString, encrypting it or storing
    /// it overly safely is not needed.
    /// </summary>
    public static class ConnectionStringManager
    {
        public static void SetConnectionString(string connectionString)
        {
            using (StreamWriter file = File.CreateText("ConnectionString.txt"))
            {
                file.Write(connectionString);
            }
        }

        public static string GetConnectionString()
        {
            using (StreamReader file = File.OpenText("ConnectionString.txt"))
            {
                var result = file.ReadToEnd();
                return result;
            }
        }

        public static async Task SetConnectionStringAsync(string connectionString)
        {
            await File.WriteAllTextAsync("ConnectionString.txt", connectionString);
        }

        public static async Task<string> GetConnectionStringAsync()
        {
            var connectionString = await File.ReadAllTextAsync("ConnectionString.txt");
            return connectionString;
        }

        /// <summary>
        /// Returns bool if connectionString is saved to system. Implies the DB
        /// has been configured.
        /// </summary>
        /// <returns>True if connectionString exists.</returns>
        public static async Task<bool> ConnectionIsConfiguredAsync()
        {
            var result = false;
            var fileExists = await Task.Run(() => File.Exists("ConnectionString.txt"));

            if (fileExists)
            {
                var content = await File.ReadAllTextAsync("ConnectionString.txt");
                
                if (content.Length > 0)
                {
                    result = true;
                }
            }
            return result;
        }

        public static bool ConnectionIsConfigured()
        {
            var result = false;
            var fileExists = File.Exists("ConnectionString.txt");

            if (fileExists)
            {
                var content = File.ReadAllText("ConnectionString.txt");

                if (content.Length > 0)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
