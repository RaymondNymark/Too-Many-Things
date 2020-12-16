using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Too_Many_Things.Core.Services
{
    /// <summary>
    /// This class saves and retrieves ConnectionString saved on a txt file.
    /// Because this is not a public connectionString, encrypting it or storing
    /// it safely is not needed.
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
    }
}
