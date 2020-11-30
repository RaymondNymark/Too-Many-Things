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

namespace Too_Many_Things.Wpf
{
    /// <summary>
    /// Interaction logic for MainControllerWindow.xaml
    /// </summary>
    public partial class MainControllerWindow : ReactiveWindow<AppBootstrapper>
    {
        public AppBootstrapper AppBootStrapper { get; protected set; }

        public MainControllerWindow()
        {
            InitializeComponent();
            AppBootStrapper = new AppBootstrapper();
            // TODO: Compile app to fix this.
            ViewModel = AppBootStrapper;
        }
    }
}
