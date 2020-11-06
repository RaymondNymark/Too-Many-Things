using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Too_Many_Things.Views;
using Too_Many_Things.ViewModels;
using Too_Many_Things.Services;

namespace Too_Many_Things
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IChecklistService _checklistService;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                _checklistService = scope.Resolve<IChecklistService>();
                // TODO1: Resolve DbContextScopeFactory.
                // TODO2: Implement startup constructors. 

                var window = new OuterView();
                window.DataContext = new ViewModelController(_checklistService, new OuterViewModel(_checklistService));
            }
        }
    }
}
