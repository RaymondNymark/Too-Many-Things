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

            // This is great.
            this.WhenActivated(disposables =>
            {
                this.WhenAnyValue(x => x.OpenSettingsCommand)
                    .BindTo(this, view => view.OpenSettingsButton.Command);
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
