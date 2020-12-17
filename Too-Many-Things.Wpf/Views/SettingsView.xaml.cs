using ReactiveUI;
using System.Reactive.Disposables;
using Too_Many_Things.Core.DataAccess.Structs;
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

            this.WhenActivated(disposables =>
            {
                // Binds the inputs to ConnectionLogin object in the VM.
                this.WhenAnyValue(
                x => x.ServerNameInput.Text,
                x => x.DatabaseNameInput.Text,
                x => x.UserNameInput.Text,
                x => x.PasswordInput.Text,
                (a, b, c, d) => new ConnectionLogin(a, b, c, d))
                .BindTo(ViewModel, x => x.ConnectionLogin)
                .DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.TestConnectionCommand,
                    v => v.TestConnectionBtn)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                    vm => vm.ConnectionStatus,
                    v => v.ConnectionStatusText.Content)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                    vm => vm.ConnectionStatusBrush,
                    v => v.ConnectionStatusText.Foreground)
                    .DisposeWith(disposables);
            });
        }
    }
}
