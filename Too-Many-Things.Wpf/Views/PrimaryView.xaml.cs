using ReactiveUI;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using Too_Many_Things.Core.ViewModels;
using Too_Many_Things.Core.Services;

namespace Too_Many_Things.Wpf.Views
{
    /// <summary>
    /// Interaction logic for PrimaryView.xaml
    /// </summary>
    public partial class PrimaryView : ReactiveUserControl<PrimaryViewModel>
    {
        public ReactiveCommand<Unit, Unit> OpenSettingsCommand { get; set; }
        public ICommand OpenSettingsICommand { get; set; }
        public PrimaryView()
        {
            InitializeComponent();
            OpenSettingsCommand = ReactiveCommand.Create(() => OpenSettingsWindow());

            this.WhenActivated(disposables =>
            {
                this.WhenAnyValue(x => x.OpenSettingsCommand)
                    .BindTo(this, view => view.OpenSettingsButton.Command);

                this.OneWayBind(ViewModel,
                    vm => vm.ConfigurationStatus,
                    v => v.ConfigurationStatusText.Text)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.RefreshCommand,
                    v => v.RefreshButton)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                    vm => vm.ChecklistCache,
                    v => v.PrimaryListBox.ItemsSource)
                    .DisposeWith(disposables);

                // Binds NewDefaultchecklistCommand to NewDefaltListButton button.
                this.BindCommand(ViewModel,
                    vm => vm.NewDefaultChecklistCommand,
                    v => v.NewDefaultListButton)
                    .DisposeWith(disposables);

                // Binds the View's SelectedItem to the ViewModel.
                this.WhenAnyValue(View => View.PrimaryListBox.SelectedItem)
                        .BindTo(this, x => x.ViewModel.SelectedChecklist)
                        .DisposeWith(disposables);

                // Binds TopViewText to selectedChecklist, purely for debugging.
                // Quicker than to write up unit test. TODO : Remove
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.SelectedChecklist.Name,
                    view => view.TopViewText.Text)
                    .DisposeWith(disposables);
            });
        }

        /// <summary>
        /// Opens the settings window.
        /// </summary>
        private void OpenSettingsWindow()
        {
            var SettingsView = new SettingsView();
            SettingsView.Show();
        }
    }
}
