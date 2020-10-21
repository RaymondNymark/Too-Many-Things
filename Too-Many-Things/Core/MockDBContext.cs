using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Too_Many_Things
{
    public class MockDBContext : IMockDBContext
    {
        private ObservableCollection<string> _mockCollection;

        public MockDBContext()
        {
            _mockCollection.Add("mockItem1");
            _mockCollection.Add("mockItem2");
            _mockCollection.Add("mockItem3");
        }

        public ObservableCollection<string> Get()
        {
            return _mockCollection;
        }
    }
}
