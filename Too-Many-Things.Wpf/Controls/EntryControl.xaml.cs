using ReactiveUI;
using System.Reactive.Disposables;
using Too_Many_Things.Core.ViewModels;

namespace Too_Many_Things.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for EntryControl.xaml
    /// </summary>
    public partial class EntryControl : ReactiveUserControl<EntryViewModel>
    {
        public EntryControl()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel,
                    vm => vm.Name,
                    v => v.EntryName.Text)
                .DisposeWith(disposables);

                this.Bind(ViewModel,
                    vm => vm.IsChecked,
                    v => v.EntryCheckBox.IsChecked)
                .DisposeWith(disposables);

                // Binds CheckboxCommand to Entrycheckbox.
                this.BindCommand(ViewModel,
                    vm => vm.CheckboxCommand,
                    v => v.EntryCheckBox)
                .DisposeWith(disposables);
            });
        }
    }
}
