using ReactiveUI;
using Too_Many_Things.Core.Services;
using Splat;
using Too_Many_Things.Wpf.Views;
using EntityFrameworkCore.DbContextScope;
using Too_Many_Things.Core.ViewModels;
using Too_Many_Things.Core.DataAccess;
using System;
using Too_Many_Things.Core.DataAccess.Models;
using Too_Many_Things.Wpf.Controls;

namespace Too_Many_Things.Wpf
{
    public class AppBootstrapper : IEnableLogger
    {
        private IMutableDependencyResolver _mutableDependencyResolver { get; set; }

        public AppBootstrapper(IMutableDependencyResolver dependencyResolver = null)
        {
            _mutableDependencyResolver = dependencyResolver ?? Locator.CurrentMutable;
        }

        /// <summary>
        /// Configures the IOC container to enable dependency injection.
        /// </summary>
        public void ConfigureIOC()
        {
            try
            {
                RegisterComponets(_mutableDependencyResolver);
            }
            catch (Exception ex)
            {
                this.Log().Error(ex, "An error occurred in configuring the IOC container. IOC has not been configured.");
            }
        }

        // Registers the dependencies all in one place. 
        private void RegisterComponets(IMutableDependencyResolver dependencyResolver)
        {   // Views + ViewModels
            dependencyResolver.Register(() => new PrimaryView(), typeof(IViewFor<PrimaryViewModel>));
            dependencyResolver.Register(() => new SecondaryView(), typeof(IViewFor<SecondaryViewModel>));
            dependencyResolver.Register(() => new SettingsView(), typeof(IViewFor<SettingsViewModel>));

            dependencyResolver.Register(() => new ListControl(), typeof(IViewFor<ListViewModel>));
            dependencyResolver.Register(() => new EntryControl(), typeof(IViewFor<EntryViewModel>));

            dependencyResolver.Register(() => new MainControllerWindow(), typeof(IViewFor<AppViewModel>));

            //// DbContexto
            dependencyResolver.Register(() => new ChecklistContextFactory(), typeof(IChecklistContextFactory));
            dependencyResolver.Register(() => new ChecklistDataService(), typeof(ChecklistDataService));

            // Services + Misc
            dependencyResolver.Register(() => new DBConnectionService(), typeof(IDBConnectionService));
        }
    }
}
