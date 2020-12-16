using ReactiveUI;
using Too_Many_Things.Core.ViewModels;

namespace Too_Many_Things.Wpf.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : ReactiveWindow<SettingsViewModel>
    {
        // TODO : Add hint / example text to boxes.
        public SettingsView()
        {
            InitializeComponent();
            ViewModel = new SettingsViewModel();

            //this.WhenActivated(disposables =>
            //{
            //});
        }
    }
}
