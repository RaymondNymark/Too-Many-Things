using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Disposables;
using Too_Many_Things.Core.DataAccess.Structs;
using Too_Many_Things.Core.ViewModels;
using static Too_Many_Things.Core.Enums.Enums;

namespace Too_Many_Things.Wpf.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : ReactiveWindow<SettingsViewModel>
    {
        [Reactive]
        public int SelectedTheme { get; set; }
        public ReactiveCommand<Unit, Unit> SwitchThemeCommand { get; set; }

        public SettingsView()
        {
            InitializeComponent();
            ViewModel = new SettingsViewModel();

            // Only lets you save and continue if theme is chosen.
            //var switchThemeCanExecute = this.WhenAnyValue(x => x.SelectedTheme,
            //    (selectedTheme) => selectedTheme > -1);
            // TODO

            SwitchThemeCommand = ReactiveCommand.Create(() => SwitchTheme(SelectedTheme));

            this.WhenActivated(disposables =>
            {
                this.WhenAnyValue(x => x.ThemeSelectionComboBox.SelectedIndex)
                    .BindTo(this, view => view.SelectedTheme);

                this.WhenAnyValue(x => x.SwitchThemeCommand)
                .BindTo(this, view => view.SaveSettingsMainButton.Command);

                // Binds the inputs to ConnectionLogin object in the VM.
                this.WhenAnyValue(
                x => x.ServerNameInput.Text,
                x => x.DatabaseNameInput.Text,
                x => x.UserNameInput.Text,
                x => x.PasswordInput.Password,
                (a, b, c, d) => new ConnectionLogin(a, b, c, d))
                .BindTo(ViewModel, x => x.ConnectionLogin)
                .DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.TestConnectionCommand,
                    v => v.TestConnectionBtn)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.ConnectSaveCommand,
                    v => v.ConnectAndSaveBtn)
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

        /// <summary>
        /// Switches the theme for the application.
        /// </summary>
        /// <param name="selectedTheme">Which theme to switch to. 0 = light, 1 = dark.</param>
        private void SwitchTheme(int selectedTheme)
        {
            var newTheme = (Theme)selectedTheme;

            (App.Current as App).ChangeTheme(newTheme);
        }
    }
}
