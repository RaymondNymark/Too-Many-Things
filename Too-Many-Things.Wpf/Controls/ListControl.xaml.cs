using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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

                // Binds EntryStringBrush to color of EntryCount label.
                this.OneWayBind(ViewModel,
                    vm => vm.EntryStringBrush,
                    v => v.EntryCount.Foreground)
                .DisposeWith(disposables);
            });
        }
    }
}
