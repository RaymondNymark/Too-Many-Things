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

namespace Too_Many_Things.Wpf.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : ReactiveWindow<SettingsViewModel>
    {
        // TODO : Add hint / example text to boxes.
        public SettingsView()
        {
            InitializeComponent();
            ViewModel = new SettingsViewModel();

            //this.WhenActivated(disposables =>
            //{
            //});
        }
    }
}
