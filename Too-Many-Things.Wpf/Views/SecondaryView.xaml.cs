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
            });
        }
    }
}
