using ReactiveUI;
using Too_Many_Things.Core.ViewModels;

namespace Too_Many_Things.Wpf.Templates
{
    /// <summary>
    /// Interaction logic for EntryTemplate.xaml
    /// </summary>
    public partial class EntryTemplate : ReactiveUserControl<EntryViewModel>
    {
        public EntryTemplate()
        {
            InitializeComponent();
        }
    }
}
