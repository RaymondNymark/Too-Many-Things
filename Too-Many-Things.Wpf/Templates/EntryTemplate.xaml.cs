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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Too_Many_Things.Core.ViewModels;

namespace Too_Many_Things.Wpf.Templates
{
    /// <summary>
    /// Interaction logic for EntryTemplate.xaml
    /// </summary>
    public partial class EntryTemplate : ReactiveUserControl<EntryViewModel>
    {
        public EntryTemplate()
        {
            InitializeComponent();
        }
    }
}
