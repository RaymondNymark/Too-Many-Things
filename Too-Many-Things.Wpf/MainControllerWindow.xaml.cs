using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
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
            // This does all of the IoC configuring. 
            AppBootStrapper = new AppBootstrapper();
            ViewModel = new AppViewModel();


            //this.WhenActivated(disposables =>
            //{

            //});
        }
    }
}
