using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Splat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Too_Many_Things.Core.DataAccess;
using static Too_Many_Things.Core.Enums.Enums;

namespace Too_Many_Things.Core.Services
{
    // Everything related to setting up custom connection strings go here.
    public partial class DataAccessService
    {
        // TODO : Check for Authentication type in overloaded
        // CreateConnectionString method.  MSAuth may be useless anyways, so it
        // defaults to SQL auth.
        private static string CreateConnectionString(string serverName, string userName = null, string password = null)
        {
            string result;
            bool usingLogin = false;

            if (userName != null && password != null)
            {
                usingLogin = true;
            }

            if (usingLogin)
            {
                result = string.Format("Server={0}; User Id={1}; Password={2};", serverName, userName, password);
            }
            else
            {
                result = string.Format("Server={0}; Trusted_Connection=True;", serverName);
            }
            return result;
        }
        private static string CreateConnectionString(string serverName, string dataBase, string userName = null, string password = null)
        {
            string result;
            bool usingLogin = false;

            if (userName != null && password != null)
            {
                usingLogin = true;
            }

            if (usingLogin)
            {
                result = string.Format("Server={0}; InitialCatlog={3}; User Id={1}; Password={2};", serverName, userName, password, dataBase);
            }
            else
            {
                result = string.Format("Server={0}; InitialCatlog={1};Trusted_Connection=True;", serverName, dataBase);
            }
            return result;
        }

        /// <summary>
        /// Tests if a server is connected.
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <returns>True if connection is opened / valid.</returns>
        private static bool IsServerConnected(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException ex)
                {
                    // TODO : Log. Splat logger method is deprecated.
                    Debug.WriteLine($"Failed to connect. Exception: {ex}");
                    return false;
                }
            }
        }

        public static bool InitializeDB(string initialConnectionString, string dataBaseName = "testDB69")
        {
            
            var options = new DbContextOptionsBuilder<ChecklistContext>()
                .UseSqlServer(initialConnectionString)
                .Options;

            using (var context = new ChecklistContext(options))
            {
                context.Database.Migrate();
            }
            //ChecklistContext Context = context ?? Locator.Current.GetService<ChecklistContext>();

            return false;
        }
    }
}
