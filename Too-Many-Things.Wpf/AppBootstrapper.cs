using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using Too_Many_Things.Services;
using Splat;
using System.Collections;
using System.Linq;
using Too_Many_Things.Wpf.Views;
using EntityFrameworkCore.DbContextScope;
using Too_Many_Things.ViewModels;

namespace Too_Many_Things.Wpf
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public RoutingState Router { get; private set; }
        private IMutableDependencyResolver _mutableDependencyResolver { get; set; }

        public AppBootstrapper(IMutableDependencyResolver dependencyResolver = null, RoutingState router = null)
        {
            Router = router ?? new RoutingState();
            _mutableDependencyResolver = dependencyResolver ?? Locator.CurrentMutable;

            // Should *probably* put any other startup tasks here:
            RegisterComponets(_mutableDependencyResolver);

            Router.Navigate.Execute(new OuterViewModel(this, Locator.Current.GetService<IChecklistService>()));
        }


        // Registers the dependencies all in one place. 
        private void RegisterComponets(IMutableDependencyResolver dependencyResolver)
        {   // Views + ViewModels
            dependencyResolver.Register(() => new OuterView(), typeof(IViewFor<OuterViewModel>));
            dependencyResolver.Register(() => new InnerView(), typeof(IViewFor<InnerViewModel>));

            // Services + Misc
            dependencyResolver.Register(() => new DbContextScopeFactory(), typeof(IDbContextScopeFactory));
            dependencyResolver.Register(() => new AmbientDbContextLocator(), typeof(IAmbientDbContextLocator));

            var dbContextScopeFactory = Locator.Current.GetService<IDbContextScopeFactory>();
            var ambientDbContextLocator = Locator.Current.GetService<IAmbientDbContextLocator>();
            dependencyResolver.Register(() => new ChecklistService(dbContextScopeFactory, ambientDbContextLocator), typeof(IChecklistService));
        }
    }
}
