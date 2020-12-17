using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Splat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Too_Many_Things.Core.DataAccess;
using Too_Many_Things.Core.DataAccess.Structs;
using static Too_Many_Things.Core.Enums.Enums;

namespace Too_Many_Things.Core.Services
{
    // Everything related to setting up custom connection strings go here.
    public partial class DataAccessService
    {
        // TODO : Check for Authentication type in overloaded
        // CreateConnectionString method.  MSAuth may be useless anyways, so it
        // defaults to SQL auth.

        /// <summary>
        /// Creates connection strings from ConnectionLogin object.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static (string pingConnectionString, string connectionString) CreateConnectionString(ConnectionLogin input)
        {
            string PingConnectionString;
            string ConnectionString;

            if (input.UserName != null && input.Password != null)
            {
                // Using login
                PingConnectionString = string.Format("Server={0}; User Id={1}; Password={2};", input.ServerName, input.UserName, input.Password);
                ConnectionString = string.Format("Server={0}; InitialCatlog={1}; User Id={2}; Password={3};", input.ServerName, input.DatabaseName, input.UserName, input.Password);
            }
            else
            {
                // Not using login
                PingConnectionString = string.Format("Server={0};Trusted_Connection=True;", input.ServerName);
                ConnectionString = string.Format("Server={0}; InitialCatlog={1};Trusted_Connection=True;", input.ServerName, input.DatabaseName);
            }

            return (PingConnectionString, ConnectionString);
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

        private static bool InitializeDB(string connectionString)
        {
            // 1: Create new DB if it doesn't exist.  Then ensure the DB is made.
            // 2: Create new connection string with new database name. 
            // 3: Save this connection string safely for WPF.
            // 4: profit

            var options = new DbContextOptionsBuilder<ChecklistContext>()
                .UseSqlServer(connectionString)
                .Options;

            // Task 1:
            CreateDbIfNotExist(options);

            // Task 2: Somehow save mainConnectionString safely on wpf.
            if (IsServerConnected(connectionString))
            {
                ConnectionStringManager.SetConnectionString(connectionString);
            }

            return false;
        }
        /// <summary>
        /// Creates DB if it doesn't already exist.
        /// </summary>
        /// <param name="options">DbContextOption parameter</param>
        /// <returns>True if it was successfully created or already exists.</returns>
        private static bool CreateDbIfNotExist(DbContextOptions options)
        {
            // Intentionally avoiding DI here for now just to make sure DI isn't
            // doing bad things here.
            using (var context = new ChecklistContext(options))
            {
                // TODO : Logging + exception catching
                try
                {
                    context.Database.Migrate();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                
            }
        }
    }
}
