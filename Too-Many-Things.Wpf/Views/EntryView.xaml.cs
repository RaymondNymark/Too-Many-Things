using ReactiveUI;
using System.Reactive.Disposables;
using Too_Many_Things.Core.ViewModels;

namespace Too_Many_Things.Wpf.Views
{
    /// <summary>
    /// Interaction logic for EntryView.xaml
    /// </summary>
    public partial class EntryView : ReactiveUserControl<IEntryViewModel>
    {
        public EntryView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel,
                    viewModel => viewModel.EntryList,
                    view => view.EntryListBox.ItemsSource)
                    .DisposeWith(disposables);

                this.BindCommand(ViewModel,
                    viewModel => viewModel.CloseChecklist,
                    view => view.GoBackButton)
                    .DisposeWith(disposables);
            });
        }
    }
}
