﻿using ReactiveUI;
using Too_Many_Things.Core.ViewModels;

namespace Too_Many_Things.Wpf.Templates
{
    /// <summary>
    /// Interaction logic for ChecklistTemplate.xaml
    /// </summary>
    public partial class ChecklistTemplate : ReactiveUserControl<ChecklistBagViewModel>
    {
        //public Checklist Checklist { get => this.DataContext as Checklist; }

        public ChecklistTemplate()
        {
            InitializeComponent();

            //var viewModel = this.DataContext as 
            //DataContext = ViewModel;

            //FullTest = this
            //    .WhenAnyValue(x => x.Checklist.Name, x => x.Checklist.ChecklistId.ToString(), (f, l) => f + ", test" + l);
            //this.DataContextChanged += (s, e) => Bindings.Update();


            //checklisthelper = Checklist.ToProperty()
            //DataContextChanged += (sender, args) => ViewModel = DataContext as ChecklistBagViewModel;


            //this.WhenActivated(disposables =>
            //{
            //    // Name attempt #1
            //    this.Bind(ViewModel,
            //        viewModel => viewModel.PublicFreeString,
            //        view => view.RealText)
            //        .DisposeWith(disposables);


            //});
        }
    }
}
