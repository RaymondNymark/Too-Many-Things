using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Too_Many_Things.Core.DataAccess;
using Too_Many_Things.Core.DataAccess.Structs;

namespace Too_Many_Things.Core.Services
{
    public interface IDBConnectionService
    {
        bool InitializeDB(string connectionString);
        Task<bool> InitializeDBAsync(string connectionString);
    }

    public class DBConnectionService : IDBConnectionService
    {
        public DBConnectionService()
        {
        }

        /// <summary>
        /// Creates connection strings from ConnectionLogin object. This does
        /// nothing to verify if the connection strings are valid or working.
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

        #region Synchronous methods.

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

        /// <summary>
        /// Initializes the dataBase by creating one if it doesn't exist and
        /// verifying that it is working.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns>True if successfully initialized</returns>
        public bool InitializeDB(string connectionString)
        {
            bool completion = false;
            bool isDBCreated = false;
            bool isDBConnectable = false;

            var options = new DbContextOptionsBuilder<ChecklistContext>()
                .UseSqlServer(connectionString)
                .Options;

            isDBCreated = CreateDbIfNotExist(options);
            isDBConnectable = IsServerConnected(connectionString);

            if (isDBCreated == true && isDBConnectable == true)
            {
                // Saves the connectionString.
                ConnectionStringManager.SetConnectionString(connectionString);
                completion = true;
            }

            return completion;
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

        #endregion
        #region Asynchronous methods
        private static async Task<bool> IsServerConnectedAsync(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
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
        private static async Task<bool> CreateDbIfNotExistsAsync(DbContextOptions options)
        {
            using (var context = new ChecklistContext(options))
            {
                try
                {
                    await context.Database.MigrateAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public async Task<bool> InitializeDBAsync(string connectionString)
        {
            bool completion = false;
            bool isDBCreated = false;
            bool isDBConnectable = false;

            var options = new DbContextOptionsBuilder<ChecklistContext>()
                .UseSqlServer(connectionString)
                .Options;

            isDBCreated = await CreateDbIfNotExistsAsync(options);
            isDBConnectable = await IsServerConnectedAsync(connectionString);

            if (isDBCreated == true && isDBConnectable == true)
            {
                // Saves the connectionString.
                await ConnectionStringManager.SetConnectionStringAsync(connectionString);
                completion = true;
            }

            return completion;
        }
        #endregion
    }
}
