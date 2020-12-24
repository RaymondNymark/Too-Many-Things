using ReactiveUI;
using System.Reactive.Disposables;
using Too_Many_Things.Core.ViewModels;

namespace Too_Many_Things.Wpf
{
    /// <summary>
    /// Interaction logic for MainControllerWindow.xaml
    /// </summary>
    public partial class MainControllerWindow : ReactiveWindow<AppViewModel>
    {
        public AppBootstrapper AppBootStrapper { get; protected set; }

        public MainControllerWindow()
        {
            InitializeComponent();

            // This configures IoC.
            AppBootStrapper = new AppBootstrapper();
            AppBootStrapper.ConfigureIOC();

            ViewModel = new AppViewModel();


            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, x => x.Router, x => x.RoutedViewHost.Router)
                    .DisposeWith(disposables);
            });
        }
    }
}
