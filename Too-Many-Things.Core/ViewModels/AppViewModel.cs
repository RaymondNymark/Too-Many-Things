using ReactiveUI;

namespace Too_Many_Things.Core.ViewModels
{
    public class AppViewModel : ReactiveObject, IScreen
    {
        public RoutingState Router { get; private set; }

        public AppViewModel(RoutingState router = null, bool UsingSqlDataBase = false)
        {
            Router = router ?? new RoutingState();

            // All other startup tasks go here:
            Router.Navigate.Execute(new PrimaryViewModel(this, null, null, UsingSqlDataBase));
        }
    }
}
