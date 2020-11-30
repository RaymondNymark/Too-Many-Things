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
    /// Interaction logic for OuterView.xaml
    /// </summary>
    public partial class OuterView : IViewFor<OuterViewModel>
    {
        public OuterView()
        {
            InitializeComponent();

            //this.WhenActivated(disposableRegistration =>
            //{
            //    this.OneWayBind(ViewModel,
            //        viewModel => viewModel.ItemSource,
            //        view => view.searchResultsListBox.ItemsSource)
            //        .DisposeWith(disposableRegistration);
            //});
        }
        public OuterViewModel ViewModel
        {
            get => (OuterViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(OuterViewModel), typeof(OuterView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (OuterViewModel)value;
        }
    }
}
