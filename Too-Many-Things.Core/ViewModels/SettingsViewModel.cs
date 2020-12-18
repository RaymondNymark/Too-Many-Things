using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
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

        private bool _testConnectionWasSuccess;

        public SettingsViewModel(RoutingState router = null, IDBConnectionService DBConnectionService = null)
        {
            Router = router ?? new RoutingState();
            _dbConectionService = DBConnectionService ?? Locator.Current.GetService<IDBConnectionService>();
            _testConnectionWasSuccess = false;

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
            // ↑ TODO : Only stay active if LoginDetails haven't changed. Actually, it doesn't matter


            // Commands
            TestConnectionCommand = ReactiveCommand.CreateFromTask(PingConnectionStringAsync, testConnectionCanExecute);
            ConnectSaveCommand = ReactiveCommand.CreateFromTask(ConnectAndSaveAsync, connectSaveCanExecute);

            // Debug-only TODO : Remove this.
            // this.WhenAnyValue(x => x.ConnectionStrings).Subscribe(x => Debug.WriteLine($"{ConnectionStrings.pingConnectionString} and {ConnectionStrings.connectionString}"));
        }

        #region Properties
        private ConnectionLogin _connectionLogin;
        private (string pingConnectionString, string connectionString) _connectionStrings;
        private string _connectionStatus;
        private Brush _connectionStatusBrush;

        public ConnectionLogin ConnectionLogin
        {
            get => _connectionLogin;
            set => this.RaiseAndSetIfChanged(ref _connectionLogin, value);
        }
        public (string pingConnectionString, string connectionString) ConnectionStrings
        {
            get => _connectionStrings;
            set => this.RaiseAndSetIfChanged(ref _connectionStrings, value);
        }

        public string ConnectionStatus
        {
            get => _connectionStatus;
            set => this.RaiseAndSetIfChanged(ref _connectionStatus, value);
        }

        public bool TestConnectionWasSuccess
        {
            get => _testConnectionWasSuccess;
            set => this.RaiseAndSetIfChanged(ref _testConnectionWasSuccess, value);
        }

        public Brush ConnectionStatusBrush
        {
            get => _connectionStatusBrush;
            set => this.RaiseAndSetIfChanged(ref _connectionStatusBrush, value);
        }
        #endregion


        #region Asynchronous Tasks & Methods
        private void UpdateConnectionStrings(ConnectionLogin connectionLogin)
        {
            ConnectionStrings = DBConnectionService.CreateConnectionString(connectionLogin);
        }

        // For color of connection status.
        public Brush GetColor()
        {
            switch (ConnectionStatus)
            {
                case "Successfully connected to the server.":
                    return Brushes.Green;
                case "Failed to connect. Please try again.":
                    return Brushes.Red;
                default:
                    return Brushes.Black;
            }
        }
 
        private async Task PingConnectionStringAsync()
        {
            ConnectionStatus = "Attempting to establish a connection...";
            ConnectionStatusBrush = GetColor();

            var status = await DBConnectionService.IsServerConnectedAsync(ConnectionStrings.pingConnectionString);

            if (status == true)
            {
                ConnectionStatus = "Successfully connected to the server.";
                ConnectionStatusBrush = GetColor();
                TestConnectionWasSuccess = true;
            }
            else
            {
                ConnectionStatus = "Failed to connect. Please try again.";
                ConnectionStatusBrush = GetColor();
                TestConnectionWasSuccess = false;
            }
        }

        private async Task ConnectAndSaveAsync()
        {
            await Task.Run(() => Debug.WriteLine("Nothing"));
        }
        #endregion


    }
}
