using System.Collections.ObjectModel;

namespace Too_Many_Things
{
    public interface IMockDBContext
    {
        ObservableCollection<string> Get();
    }
}