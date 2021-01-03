using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Threading;
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

        private void OnMinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            // This is a hacky solution to have an animation when a window is
            // minimized.  It changes WindowStyle to SingleBorderWindow and
            // changes back to none once it's been opened again.
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.WindowState = WindowState.Minimized;
        }
        private void MainWindow_OnActivated(object sender, EventArgs e)
        {
            //change the WindowStyle back to None, but only after the Window has been activated
            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => WindowStyle = WindowStyle.None));
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
