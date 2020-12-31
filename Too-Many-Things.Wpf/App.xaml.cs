using ReactiveUI;
using Splat;
using System;
using System.Reflection;
using System.Windows;
using Props = Too_Many_Things.Wpf.Properties;
using static Too_Many_Things.Core.Enums.Enums;

namespace Too_Many_Things.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Theme Theme { get; set; } = Theme.Light;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Retrieves the int value of the savedTheme user-setting and
            // applies the theme at startup. Default will be light theme.
            ChangeTheme((Theme)Props.Settings.Default.SavedTheme);
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

        // Applies the dark theme to current session and saves it to settings.
        private void ApplyDarkTheme()
        {
            AddResourceDictionary("Themes\\DarkTheme.xaml");
            Props.Settings.Default.SavedTheme = 1;
            Props.Settings.Default.Save();
        }

        // Applies the dark theme to current session and saves it to settings.
        private void ApplyLightTheme()
        {
            AddResourceDictionary("Themes\\LightTheme.xaml");
            Props.Settings.Default.SavedTheme = 0;
            Props.Settings.Default.Save();
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
