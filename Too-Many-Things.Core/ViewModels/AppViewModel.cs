using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;

namespace Too_Many_Things.Core.ViewModels
{
    public class AppViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; private set; }

        public AppViewModel(RoutingState router = null)
        {
            Router = router ?? new RoutingState();

            // All other startup tasks go here:

            Router.Navigate.Execute(new ChecklistBagViewModel());

            // Router.Navigate.Execute(new anyViewModel()) to fire off first default viewModel.
            OpenView = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new ChecklistBagViewModel()));
        }

        // All commands should go here.
        public ReactiveCommand<Unit, IRoutableViewModel> OpenView { get; }
    }
}
