using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Too_Many_Things.Core.ViewModels
{
    public class SettingsViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; private set; }

        public SettingsViewModel(RoutingState router = null)
        {
            Router = router ?? new RoutingState();

            // Router.Navigate.Execute(new anyViewModel()) to fire off first default viewModel.
            //OpenView = ReactiveCommand.CreateFromObservable(() => Router.Navigate.Execute(new ChecklistBagViewModel()));
        }
    }
}
