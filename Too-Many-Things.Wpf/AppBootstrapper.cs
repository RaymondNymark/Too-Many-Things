using ReactiveUI;
using Too_Many_Things.Core.Services;
using Splat;
using Too_Many_Things.Wpf.Views;
using EntityFrameworkCore.DbContextScope;
using Too_Many_Things.Core.ViewModels;
using Too_Many_Things.Core.DataAccess;
using System;

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

        /// <summary>
        /// Configures the DataBase. Creates new DataBase is it doesn't exist.
        /// </summary>
        public void ConfigureDB()
        {
            CreateDBIfNotExists();
        }

        // Registers the dependencies all in one place. 
        private void RegisterComponets(IMutableDependencyResolver dependencyResolver)
        {   // Views + ViewModels
            dependencyResolver.Register(() => new ChecklistBagView(), typeof(IViewFor<ChecklistBagViewModel>));
            dependencyResolver.Register(() => new PrimaryView(), typeof(IViewFor<PrimaryViewModel>));
            dependencyResolver.Register(() => new EntryView(), typeof(IViewFor<EntryViewModel>));
            dependencyResolver.Register(() => new SettingsView(), typeof(IViewFor<SettingsViewModel>));

            // TODO : Figure out if this needs to actually be here.
            dependencyResolver.Register(() => new MainControllerWindow(), typeof(IViewFor<AppViewModel>));

            //// DbContexto
            dependencyResolver.Register(() => new ChecklistContextFactory(), typeof(IChecklistContextFactory));

            // Services + Misc
            dependencyResolver.Register(() => new DBConnectionService(), typeof(IDBConnectionService));

            dependencyResolver.Register(() => new DbContextScopeFactory(), typeof(IDbContextScopeFactory));
            dependencyResolver.Register(() => new AmbientDbContextLocator(), typeof(IAmbientDbContextLocator));

            var dbContextScopeFactory = Locator.Current.GetService<IDbContextScopeFactory>();
            var ambientDbContextLocator = Locator.Current.GetService<IAmbientDbContextLocator>();
            dependencyResolver.Register(() => new ChecklistService(dbContextScopeFactory, ambientDbContextLocator), typeof(IChecklistService));
        }

        private void CreateDBIfNotExists()
        {
            try
            {
                var context = Locator.Current.GetService<ChecklistContext>();
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                this.Log().Error(ex, "An error occurred in creating the local DataBase");
            }
        }
    }
}
