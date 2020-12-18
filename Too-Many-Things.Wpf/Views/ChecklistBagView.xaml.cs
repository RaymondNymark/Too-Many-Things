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
    /// Interaction logic for ChecklistBagView.xaml
    /// </summary>
    public partial class ChecklistBagView : ReactiveUserControl<IChecklistBagViewModel>
    {
        public ReactiveCommand<Unit, Unit> OpenSettingsCommand { get; set; }
        public ICommand OpenSettingsICommand { get; set; }

        public ChecklistBagView()
        {
            InitializeComponent();
            OpenSettingsCommand = ReactiveCommand.Create(() => OpenSettingsWindow());
            OpenSettingsICommand = OpenSettingsCommand;
            
            // This is great.
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.ChecklistList,
                    view => view.ChecklistListBox.ItemsSource)
                    .DisposeWith(disposables);

                // Binds TopViewText to selectedChecklist, purely for debugging.
                // Quicker than to write up unit test. TODO : Remove
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.SelectedChecklist.Name,
                    view => view.TopViewText.Text)
                    .DisposeWith(disposables);

                // Binds the View's SelectedItem to the ViewModel.
                this.WhenAnyValue(View => View.ChecklistListBox.SelectedItem)
                        .BindTo(this, x => x.ViewModel.SelectedChecklist)
                        .DisposeWith(disposables);

                // Binds OpenChecklist command to OpenChecklistButton.
                this.BindCommand(ViewModel,
                    viewModel => viewModel.OpenChecklist,
                    view => view.OpenChecklistButton)
                    .DisposeWith(disposables);

                // These three sections implement ability to double click on a
                // checklist to open it. Executes OpenChecklist command when an
                // item in item collection is double clicked.  
                var mouseUp = Observable.FromEventPattern<MouseButtonEventHandler,
                    MouseButtonEventArgs>
                    (h => MouseLeftButtonUp += h, h => MouseLeftButtonUp -= h);

                var doubleClick = mouseUp.SelectMany(
                    e => mouseUp.Take(1).Timeout(
                         TimeSpan.FromMilliseconds(350),
                        Observable.Empty<EventPattern<MouseButtonEventArgs>>()));

                doubleClick.Select(_ => this.ChecklistListBox.SelectedItem)
                   .Where(x => x != null)
                   .Subscribe(x => ViewModel.OpenChecklist.Execute());

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
