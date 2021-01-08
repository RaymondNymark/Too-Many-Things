using System;
using System.Collections.Generic;
using System.Text;
using Too_Many_Things.Core.DataAccess.Structs;
using Too_Many_Things.Core.Services;
using Xunit;

namespace Too_Many_Things.Tests.Core.Services
{
    public class DBConnectionServiceTests
    {
        [Fact]
        public void CreateConnectionString_CreatesExpectedConnectionStrings()
        {
            var withoutLoginInput = new ConnectionLogin("TestServerName", "TestDataBaseName");
            var withLoginInput = new ConnectionLogin("TestServerName", "TestDataBaseName", "UserName", "Password");

            string expectedConnectionStringWithoutLoginInput = "Server=TestServerName; Database=TestDataBaseName;Trusted_Connection=True;";
            string expectedConnectionStringWithLoginInput = "Server=TestServerName; Database=TestDataBaseName; User Id=UserName; Password=Password;";

            var actualWithoutLogin = DBConnectionService.CreateConnectionString(withoutLoginInput);
            var actualWithLogin = DBConnectionService.CreateConnectionString(withLoginInput);

            Assert.Equal(expectedConnectionStringWithoutLoginInput, actualWithoutLogin.connectionString);
            Assert.Equal(expectedConnectionStringWithLoginInput, actualWithLogin.connectionString);
        }


    }
}
