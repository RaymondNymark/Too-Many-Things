﻿using ReactiveUI;
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

namespace Too_Many_Things.Wpf.Controls
{
    /// <summary>
    /// Interaction logic for EntryControl.xaml
    /// </summary>
    public partial class EntryControl : ReactiveUserControl<EntryViewModel>
    {
        public EntryControl()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel,
                    vm => vm.Name,
                    v => v.EntryName.Text)
                .DisposeWith(disposables);

                this.Bind(ViewModel,
                    vm => vm.IsChecked,
                    v => v.EntryCheckBox.IsChecked)
                .DisposeWith(disposables);
            });
        }
    }
}
