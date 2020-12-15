using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Too_Many_Things.Core.ViewModels
{
    public class SettingsViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; private set; }

        public SettingsViewModel(RoutingState router = null)
        {
            Router = router ?? new RoutingState();
        }
    }
}
