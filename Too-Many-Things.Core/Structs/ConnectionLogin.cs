using System;
using System.Collections.Generic;
using System.Text;

namespace Too_Many_Things.Core.DataAccess.Structs
{
    public struct ConnectionLogin
    {
        public string ServerName { get; }
        public string DatabaseName { get; }
        public string UserName { get; }
        public string Password { get; }

        public ConnectionLogin(string serverName, string databaseName, string userName = null, string password = null)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
            UserName = userName;
            Password = password;
        }
    }
}
