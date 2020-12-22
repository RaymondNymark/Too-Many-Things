using ReactiveUI;
using System.Diagnostics;
using System.Reactive.Disposables;
using Too_Many_Things.Core.DataAccess.Models;

namespace Too_Many_Things.Wpf.Templates
{
    /// <summary>
    /// Interaction logic for ListTemplate.xaml
    /// </summary>
    public partial class ListTemplate : ReactiveUserControl<List>
    {
        public ListTemplate()
        {
            InitializeComponent();
        }
    }
}
