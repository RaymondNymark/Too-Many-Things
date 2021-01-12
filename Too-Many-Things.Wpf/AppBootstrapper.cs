using ReactiveUI;
using Splat;
using System;
using Too_Many_Things.Core.DataAccess;
using Too_Many_Things.Core.Services;
using Too_Many_Things.Core.ViewModels;
using Too_Many_Things.Wpf.Controls;
using Too_Many_Things.Wpf.Views;

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

        // Registers all of the dependencies in one place!
        private void RegisterComponets(IMutableDependencyResolver dependencyResolver)
        {   // Views + ViewModels
            dependencyResolver.Register(() => new PrimaryView(), typeof(IViewFor<PrimaryViewModel>));
            dependencyResolver.Register(() => new SecondaryView(), typeof(IViewFor<SecondaryViewModel>));
            dependencyResolver.Register(() => new SettingsView(), typeof(IViewFor<SettingsViewModel>));

            dependencyResolver.Register(() => new ListControl(), typeof(IViewFor<ListViewModel>));
            dependencyResolver.Register(() => new EntryControl(), typeof(IViewFor<EntryViewModel>));

            dependencyResolver.Register(() => new MainControllerWindow(), typeof(IViewFor<AppViewModel>));

            // DbContexts
            dependencyResolver.Register(() => new ChecklistContextFactory(), typeof(IChecklistContextFactory));
            dependencyResolver.Register(() => new ChecklistDataService(), typeof(IChecklistDataService));

            // Services + Misc
            dependencyResolver.Register(() => new DBConnectionService(), typeof(IDBConnectionService));
            dependencyResolver.Register(() => new LocalDataStorageService(), typeof(ILocalDataStorageService));
        }
    }
}
