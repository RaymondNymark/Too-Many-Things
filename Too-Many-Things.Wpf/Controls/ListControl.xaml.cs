using ReactiveUI;
using System.Reactive.Disposables;
using Too_Many_Things.Core.ViewModels;

namespace Too_Many_Things.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for ListControl.xaml
    /// </summary>
    public partial class ListControl : ReactiveUserControl<ListViewModel>
    {
        public ListControl()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel,
                    vm => vm.Name,
                    v => v.MainListName.Text)
                .DisposeWith(disposables);

                // Binds EntryString to EntryCount label text.
                this.OneWayBind(ViewModel,
                    vm => vm.EntryString,
                    v => v.EntryCount.Text)
                .DisposeWith(disposables);

                // Binds Opacity from VM to view label.
                this.OneWayBind(ViewModel,
                    vm => vm.Opacity,
                    v => v.EntryCount.Opacity)
                .DisposeWith(disposables);
            });
        }
    }
}
