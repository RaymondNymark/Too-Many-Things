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

namespace Too_Many_Things.ViewModels
{
    public class AppBootstrapper : ReactiveObject
    {
        public RoutingState Router { get; private set; }
        private IMutableDependencyResolver _mutableDependencyResolver { get; set; }

        public AppBootstrapper(IMutableDependencyResolver dependencyResolver = null, RoutingState router = null)
        {
            Router = router ?? new RoutingState();
            _mutableDependencyResolver = dependencyResolver ?? Locator.CurrentMutable;

            // Should probably put any other startup tasks here:
            RegisterComponets(_mutableDependencyResolver);

            // TODO: Resolve this
            //Router.Navigate.Execute(new OuterViewModel());
        }


        // Registers the dependancies all in one place. 
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

        //private IChecklistService _checklistService;
        //public RoutingState Router { get; private set; }

        //public AppBootstrapper(IContainer container = null, RoutingState router = null)
        //{
        //    Router = router ?? new RoutingState();
        //    container = container ?? ContainerConfig.Configure();

        //    var autofacResolver = container.Resolve<AutofacDependencyResolver>();

        //    autofacResolver.SetLifetimeScope(container);

        //    using (var scope = container.BeginLifetimeScope())
        //    {
        //        _checklistService = scope.Resolve<IChecklistService>();

        //        // Navigate to the opening page of this application.
        //        Router.Navigate.Execute(new OuterViewModel(_checklistService));
        //    }
        //}
    }

    //public class AutofacDependencyResolver : IMutableDependencyResolver
    //{
    //    private readonly IContainer _container;

    //    public AutofacDependencyResolver(IContainer container)
    //    {
    //        _container = container;
    //    }

    //    public object GetService(Type serviceType, string contract = null)
    //    {
    //        try
    //        {
    //            return string.IsNullOrEmpty(contract)
    //                ? _container.Resolve(serviceType)
    //                : _container.ResolveNamed(contract, serviceType);
    //        }
    //        catch(DependencyResolutionException)
    //        {
    //            return null;
    //        }
    //    }

    //    public IEnumerable<object> GetServices(Type serviceType, string contract = null)
    //    {
    //        try
    //        {
    //            var enumerableType = typeof(IEnumerable<>).MakeGenericType(serviceType);
    //            object instance = string.IsNullOrEmpty(contract)
    //                ? _container.Resolve(enumerableType)
    //                : _container.ResolveNamed(contract, enumerableType);
    //            return ((IEnumerable)instance).Cast<object>();
    //        }
    //        catch (DependencyResolutionException)
    //        {
    //            return null;
    //        }
    //    }

    //    public void Register(Func<object> factory, Type serviceType, string contract = null)
    //    {
    //        var builder = new ContainerBuilder();
    //        if (string.IsNullOrEmpty(contract))
    //        {
    //            builder.Register(x => factory()).As(serviceType).AsImplementedInterfaces();
    //        }
    //        else
    //        {
    //            builder.Register(x => factory()).Named(contract, serviceType).AsImplementedInterfaces();
    //        }
    //    }

    //    public IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Dispose()
    //    {
    //        _container.Dispose();
    //    }

    //    public bool HasRegistration(Type serviceType, string contract = null)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void UnregisterCurrent(Type serviceType, string contract = null)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void UnregisterAll(Type serviceType, string contract = null)
    //    {
    //        throw new NotImplementedException();
    //    }
    }
}
