using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Too_Many_Things.Core.DataAccess.Structs;
using Too_Many_Things.Core.Services;

namespace Too_Many_Things.Core.ViewModels
{
    public class SettingsViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; private set; }
        private IDBConnectionService _dbConectionService;
        public ReactiveCommand<Unit, Unit> TestConnectionCommand;
        public ReactiveCommand<Unit, Unit> ConnectSaveCommand;
        public ReactiveCommand<Unit, Unit> ToggleUseSqlDataBaseCommand;

        private bool _testConnectionWasSuccess;

        public SettingsViewModel(bool UsingSqlDataBase, RoutingState router = null, IDBConnectionService DBConnectionService = null)
        {
            Router = router ?? new RoutingState();
            _dbConectionService = DBConnectionService ?? Locator.Current.GetService<IDBConnectionService>();
            _testConnectionWasSuccess = false;
            UsingSqlDatabase = UsingSqlDataBase;

            // Updates connection strings with a 1000ms throttle when anything is entered into settings.
            this.WhenAnyValue(x => x.ConnectionLogin)
                .Throttle(TimeSpan.FromMilliseconds(1000))
                .DistinctUntilChanged()
                .Subscribe(x => UpdateConnectionStrings(ConnectionLogin));

            var testConnectionCanExecute = this.WhenAnyValue(
                x => x.ConnectionLogin.ServerName,
                x => x.ConnectionLogin.DatabaseName,
                (serverName, databaseName) =>
                    !string.IsNullOrEmpty(serverName) &&
                    !string.IsNullOrEmpty(databaseName));

            var connectSaveCanExecute = this.WhenAnyValue(
                x => x.TestConnectionWasSuccess,
                (flag) => flag == true);

            // Commands
            TestConnectionCommand = ReactiveCommand.CreateFromTask(PingConnectionStringAsync, testConnectionCanExecute);
            ConnectSaveCommand = ReactiveCommand.CreateFromTask(ConnectAndSaveAsync, connectSaveCanExecute);
            ToggleUseSqlDataBaseCommand = ReactiveCommand.Create(ToggleUseSqlDatabase);
        }

        #region Properties

        [Reactive]
        public ConnectionLogin ConnectionLogin { get; set; }
        [Reactive]
        public (string pingConnectionString, string connectionString) ConnectionStrings { get; set; }
        [Reactive]
        public string ConnectionStatus { get; set; }

        [Reactive]
        public string ConnectionStatusHex { get; set; } = "#39FF14";

        [Reactive]
        public bool UsingSqlDatabase { get; set; }

        public bool TestConnectionWasSuccess
        {
            get => _testConnectionWasSuccess;
            set => this.RaiseAndSetIfChanged(ref _testConnectionWasSuccess, value);
        }
        #endregion


        #region Asynchronous Tasks & Methods
        // This method keeps the connection string constantly updated when the input changes.
        private void UpdateConnectionStrings(ConnectionLogin connectionLogin)
        {
            ConnectionStrings = DBConnectionService.CreateConnectionString(connectionLogin);
        }

        /// <summary>
        /// Pings the stored connection string to see if it's valid.
        /// </summary>
        /// <returns></returns>
        private async Task PingConnectionStringAsync()
        {
            ConnectionStatus = "Attempting to establish a connection...";
            ConnectionStatusHex = "#000000"; // Black color

            var status = await DBConnectionService.IsServerConnectedAsync(ConnectionStrings.pingConnectionString);

            if (status == true)
            {
                ConnectionStatus = "Successfully connected to the server.";
                ConnectionStatusHex = "#00a637"; // Green Color
                TestConnectionWasSuccess = true;
            }
            else
            {
                ConnectionStatus = "Failed to connect. Please try again.";
                ConnectionStatusHex = "#FF0000"; // Red Color
                TestConnectionWasSuccess = false;
            }
        }

        /// <summary>
        /// Initializes the database using the saved connection string.  This
        /// creates the database behind the scenes.
        /// </summary>
        private async Task ConnectAndSaveAsync()
        {
            var success = false;
            ConnectionStatusHex = "#000000"; // Black color
            ConnectionStatus = "Trying to initialize and save the connection...";
            try
            {
                success = await _dbConectionService.InitializeDBAsync(ConnectionStrings.connectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (success == true)
            {
                ConnectionStatusHex = "#0033ff"; // Blue Color
                ConnectionStatus = "Connection has been initialized and saved!";
            }
            else
            {
                ConnectionStatusHex = "#FF0000"; // Red Color
                ConnectionStatus = "Saving has catastrophically failed.";
            }
        }

        /// <summary>
        /// Toggles UsingSqlDatabase flag to opposite.
        /// </summary>
        private void ToggleUseSqlDatabase()
        {
            if (UsingSqlDatabase)
            {
                UsingSqlDatabase = false;
            }
            else
            {
                UsingSqlDatabase = true;
            }
        }
        #endregion


    }
}
