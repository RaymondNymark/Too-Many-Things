using ReactiveUI;
using Too_Many_Things.Core.Services;
using Splat;
using Too_Many_Things.Wpf.Views;
using EntityFrameworkCore.DbContextScope;
using Too_Many_Things.Core.ViewModels;

namespace Too_Many_Things.Wpf
{
    public class AppBootstrapper
    {
        private IMutableDependencyResolver _mutableDependencyResolver { get; set; }

        public AppBootstrapper(IMutableDependencyResolver dependencyResolver = null)
        {
            _mutableDependencyResolver = dependencyResolver ?? Locator.CurrentMutable;
            RegisterComponets(_mutableDependencyResolver);
        }

        // Registers the dependencies all in one place. 
        private void RegisterComponets(IMutableDependencyResolver dependencyResolver)
        {   // Views + ViewModels
            dependencyResolver.Register(() => new OuterView(), typeof(IViewFor<OuterViewModel>));
            dependencyResolver.Register(() => new InnerView(), typeof(IViewFor<InnerViewModel>));
            dependencyResolver.Register(() => new ChecklistBagView(), typeof(IViewFor<ChecklistBagViewModel>));
            dependencyResolver.Register(() => new EntryView(), typeof(IViewFor<EntryViewModel>));

            // TODO : Figure out if this needs to actually be here.
            dependencyResolver.Register(() => new MainControllerWindow(), typeof(IViewFor<AppViewModel>));

            // Services + Misc
            dependencyResolver.Register(() => new DbContextScopeFactory(), typeof(IDbContextScopeFactory));
            dependencyResolver.Register(() => new AmbientDbContextLocator(), typeof(IAmbientDbContextLocator));

            var dbContextScopeFactory = Locator.Current.GetService<IDbContextScopeFactory>();
            var ambientDbContextLocator = Locator.Current.GetService<IAmbientDbContextLocator>();
            dependencyResolver.Register(() => new ChecklistService(dbContextScopeFactory, ambientDbContextLocator), typeof(IChecklistService));
        }
    }
}
