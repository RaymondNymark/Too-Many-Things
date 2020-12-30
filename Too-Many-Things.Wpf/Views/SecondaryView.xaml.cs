using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Too_Many_Things.Core.ViewModels;

namespace Too_Many_Things.Wpf.Views
{
    /// <summary>
    /// Interaction logic for SecondaryView.xaml
    /// </summary>
    public partial class SecondaryView : ReactiveUserControl<SecondaryViewModel>
    {
        public SecondaryView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                // Binds BindingEntryCache to ItemSource of the view.
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.BindingEntryCache,
                    view => view.SecondaryListBox.ItemsSource)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    viewModel => viewModel.GoBackToPrimaryView,
                    view => view.GoBackButton)
                    .DisposeWith(disposables);

                // Binds name of checklist to top of UI.
                this.OneWayBind(ViewModel,
                    vm => vm.SelectedList.Name,
                    v => v.ChecklistNameText.Text)
                .DisposeWith(disposables);

                // Binds selected Entry to SelectedEntryProperty in VM
                this.WhenAnyValue(View => View.SecondaryListBox.SelectedItem)
                        .BindTo(this, x => x.ViewModel.SelectedEntry)
                        .DisposeWith(disposables);


                // Binds ViewModel property to label text, so on rename or
                // deletion it explicitly tells you which entity you're
                // deleting.
                this.OneWayBind(ViewModel,
                    vm => vm.ChangeNameString,
                    v => v.ChangeName_text.Content)
                .DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                    vm => vm.DeleteObjectString,
                    v => v.DeletionInfoText.Content)
                .DisposeWith(disposables);

                // Binds newEntryCommand to new entry button.
                this.BindCommand(ViewModel,
                    vm => vm.NewDefaultEntryCommand,
                    v => v.NewEntryButton)
                .DisposeWith(disposables);

                // Bind Edit and Done Button's to a bool in VM.
                this.OneWayBind(ViewModel,
                    vm => vm.EditModeIsDisabled,
                    v => v.EditButton.Visibility)
                .DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                    vm => vm.EditModeIsEnabled,
                    v => v.FinishEditButton.Visibility)
                .DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                    vm => vm.EditModeIsEnabled,
                    v => v.NewEntryButton.Visibility)
                .DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.TurnEntryEditOnCommand,
                    v => v.EditButton)
                .DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.TurnEntryEditOffCommand,
                    v => v.FinishEditButton)
                .DisposeWith(disposables);

                // These bind the same command to check and uncheck all buttons.
                this.BindCommand(ViewModel,
                    vm => vm.CheckEveryEntry,
                    v => v.CheckAllButton,
                    vm => vm.TrueBool)
                .DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    vm => vm.CheckEveryEntry,
                    v => v.UncheckAllButton,
                    vm => vm.FalseBool)
                .DisposeWith(disposables);



                #region Properties for edit and deletion with the contextMenu.
                // Context menu commands
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
                    v => v.SecondaryGrid.Opacity)
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
                    v => v.SecondaryGrid.IsHitTestVisible,
                    boolValue => !boolValue)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel,
                    vm => vm.IsDeleting,
                    v => v.SecondaryGrid.IsHitTestVisible,
                    boolValue => !boolValue)
                    .DisposeWith(disposables);

                // Two-way bind between ChangeName_box.text and RenameListInput
                // so it can be cleared when re-opened.
                this.Bind(ViewModel,
                    vm => vm.RenameEntryInput,
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
                #endregion
            });
        }
    }
}
