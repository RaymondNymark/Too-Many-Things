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
using static Too_Many_Things.Core.Enums.Enums;

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
                        .BindTo(this, x => x.ViewModel.SelectedList)
                        .DisposeWith(disposables);

                // Binds TopViewText to selectedChecklist, purely for debugging.
                // Quicker than to write up unit test. TODO : Remove
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.SelectedList.Name,
                    view => view.TopViewText.Text)
                    .DisposeWith(disposables);

                this.WhenAnyValue(x => x.OpenSettingsCommand)
                    .BindTo(this, view => view.OpenSettingsButton.Command);

                #region Double Click functionality
                // These three sections implement ability to double click on a
                // checklist to open it. Executes OpenList command when an
                // item in item collection is double clicked.  
                var mouseUp = Observable.FromEventPattern<MouseButtonEventHandler,
                    MouseButtonEventArgs>
                    (h => MouseLeftButtonUp += h, h => MouseLeftButtonUp -= h);

                var doubleClick = mouseUp.SelectMany(
                    e => mouseUp.Take(1).Timeout(
                         TimeSpan.FromMilliseconds(350),
                        Observable.Empty<EventPattern<MouseButtonEventArgs>>()));

                doubleClick.Select(_ => this.PrimaryListBox.SelectedItem)
                   .Where(x => x != null)
                   .Subscribe(x => ViewModel.OpenList.Execute());
                #endregion

                #region Context-Menu functionality
                this.BindCommand(ViewModel,
                    vm => vm.EnableEditCommand,
                    v => v.RenameMenuItem,
                    vm => vm.RenamingState)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.EnableEditCommand,
                    v => v.DeleteMenuItem,
                    vm => vm.DeletingState)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                    vm => vm.GridOppacity,
                    v => v.PrimaryGrid.Opacity)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                    vm => vm.IsRenaming,
                    v => v.RenameConfirmationGrid.Visibility)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                    vm => vm.IsDeleting,
                    v => v.DeleteConfirmationGrid.Visibility)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                    vm => vm.IsRenaming,
                    v => v.PrimaryGrid.IsHitTestVisible,
                    boolValue => !boolValue)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                    vm => vm.IsDeleting,
                    v => v.PrimaryGrid.IsHitTestVisible,
                    boolValue => !boolValue)
                    .DisposeWith(disposables);

                // Two-way bind between ChangeName_box.text and RenameListInput
                // so it can be cleared when re-opened.
                this.Bind(ViewModel,
                    vm => vm.RenameListInput,
                    v => v.ChangeName_box.Text)
                .DisposeWith(disposables);

                // Binds CancelEditCommand to cancel buttons:
                this.BindCommand(ViewModel,
                    vm => vm.CancelEditCommand,
                    v => v.CancelRenameButton)
                .DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.CancelEditCommand,
                    v => v.CancelDeletionButton)
                .DisposeWith(disposables);

                // Binds confirmation buttons:
                this.BindCommand(ViewModel,
                    vm => vm.ConfirmRenameCommand,
                    v => v.ConfirmRenameButton)
                .DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.ConfirmDeletionCommand,
                    v => v.ConfirmDeletionButton)
                .DisposeWith(disposables);

                // TODO : Add selected checklist name.
                //this.OneWayBind(ViewModel,
                //    vm => "Change the name of a checklist",
                //    v => v.ChangeName_text.Content)
                //.DisposeWith(disposables);
                #endregion
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
