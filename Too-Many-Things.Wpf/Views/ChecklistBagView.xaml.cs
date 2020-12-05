using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
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
using ReactiveUI;
using Too_Many_Things.Core.Models;
using Too_Many_Things.Core.ViewModels;
using Too_Many_Things.Wpf.Templates;

namespace Too_Many_Things.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ChecklistBagView.xaml
    /// </summary>
    public partial class ChecklistBagView : ReactiveUserControl<ChecklistBagViewModel>
    {
        public ChecklistBagView()
        {
            InitializeComponent();

            //var mouseUp = Observable.FromEventPattern<MouseButtonEventHandler,
            //    MouseButtonEventArgs>
            //    (h => MouseLeftButtonUp += h, h => MouseLeftButtonUp -= h);

            //var doubleClick = mouseUp.SelectMany(
            //    e => mouseUp.Take(1).Timeout(
            //        TimeSpan.FromMilliseconds(500),
            //        Observable.Empty<EventPattern<MouseButtonEventArgs>>()));

            //doubleClick.Select(_ => this.ChecklistListBox.SelectedItem)
            //    .Where(x => x != null)
            //    .Subscribe(x => ((MyViewModel)DataContext).MyCommand.Execute(null));


            // Love this
            this.WhenActivated(disposables =>
            {
                // ItemSource
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.TestItemSource,
                    view => view.ChecklistListBox.ItemsSource)
                    .DisposeWith(disposables);
            });
        }
    }
}
