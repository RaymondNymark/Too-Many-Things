using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
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
        public Brush ConnectionStatusBrush { get; set; }

        [Reactive]
        public bool UsingSqlDatabase { get; set; }

        public bool TestConnectionWasSuccess
        {
            get => _testConnectionWasSuccess;
            set => this.RaiseAndSetIfChanged(ref _testConnectionWasSuccess, value);
        }
        #endregion


        #region Asynchronous Tasks & Methods
        private void UpdateConnectionStrings(ConnectionLogin connectionLogin)
        {
            ConnectionStrings = DBConnectionService.CreateConnectionString(connectionLogin);
        }

        private async Task PingConnectionStringAsync()
        {
            ConnectionStatus = "Attempting to establish a connection...";
            ConnectionStatusBrush = Brushes.Black;

            var status = await DBConnectionService.IsServerConnectedAsync(ConnectionStrings.pingConnectionString);

            if (status == true)
            {
                ConnectionStatus = "Successfully connected to the server.";
                ConnectionStatusBrush = Brushes.Green;
                TestConnectionWasSuccess = true;
            }
            else
            {
                ConnectionStatus = "Failed to connect. Please try again.";
                ConnectionStatusBrush = Brushes.Red;
                TestConnectionWasSuccess = false;
            }
        }

        private async Task ConnectAndSaveAsync()
        {
            var success = false;
            ConnectionStatusBrush = Brushes.Black;
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
                ConnectionStatusBrush = Brushes.Blue;
                ConnectionStatus = "Connection has been initialized and saved!";
            }
            else
            {
                ConnectionStatusBrush = Brushes.Red;
                ConnectionStatus = "Saving has catastrophically failed.";
            }
        }

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
