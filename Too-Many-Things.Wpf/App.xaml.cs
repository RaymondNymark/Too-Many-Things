using ReactiveUI;
using Splat;
using System.Reflection;
using System.Windows;

namespace Too_Many_Things.Wpf
{
    public enum Theme { Light, Dark};

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Theme Theme { get; set; } = Theme.Dark;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ChangeTheme(Theme.Dark);
        }

        public void ChangeTheme(Theme newTheme)
        {
            Theme = newTheme;
            Resources.Clear();
            Resources.MergedDictionaries.Clear();

            if (Theme == Theme.Dark)
                ApplyDarkTheme();
            else if (Theme == Theme.Light)
                ApplyLightTheme();

            ApplySharedResources();
        }

        private void ApplyDarkTheme()
        {

        }

        private void ApplyLightTheme()
        {

        }

        private void ApplySharedResources()
        {
            AddResourceDictionary("Themes\\Shared.xaml");
        }
        
        private void AddResourceDictionary(string src)
        {
            Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new System.Uri(src, System.UriKind.Relative) });
        }
    }
}
