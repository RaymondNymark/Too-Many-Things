using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Too_Many_Things.Core.DataAccess.Structs;
using Too_Many_Things.Core.Services;

namespace Too_Many_Things.Core.ViewModels
{
    public class SettingsViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; private set; }
        private IDBConnectionService _dbConectionService;

        public SettingsViewModel(RoutingState router = null, IDBConnectionService DBConnectionService = null)
        {
            Router = router ?? new RoutingState();
            _dbConectionService = DBConnectionService ?? Locator.Current.GetService<IDBConnectionService>();

            // Updates connection strings with a 1000ms throttle when anything is entered into settings.
            this.WhenAnyValue(x => x.ConnectionLogin)
                .Throttle(TimeSpan.FromMilliseconds(1000))
                .DistinctUntilChanged()
                .Subscribe(x => UpdateConnectionStrings(ConnectionLogin));

            // Debug-only TODO : Remove this.
            //this.WhenAnyValue(x => x.ConnectionStrings).Subscribe(x => Debug.WriteLine($"{ConnectionStrings.pingConnectionString} and {ConnectionStrings.connectionString}"));
        }

        #region Properties
        private ConnectionLogin _connectionLogin;
        private (string pingConnectionString, string connectionString) _connectionStrings; 

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
        #endregion

        private void UpdateConnectionStrings(ConnectionLogin connectionLogin)
        {
            ConnectionStrings = DBConnectionService.CreateConnectionString(connectionLogin);
        }

        #region Asynchronous Tasks
        //private async Task<bool> TestConnectionAsync()
        //{
        //    var connectionLogin = new ConnectionLogin(ServerName, DatabaseName, UserName, Password);
        //    var conStrings = DBConnectionService.CreateConnectionString(connectionLogin);

        //    ConnectionString = conStrings.connectionString;
        //    MiniConnectionString = conStrings.pingConnectionString;

        //    return await DBConnectionService.IsServerConnectedAsync(MiniConnectionString);
        //}

        #endregion


    }
}
