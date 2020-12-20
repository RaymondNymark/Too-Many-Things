using System;
using System.Collections.Generic;
using System.Text;
using Too_Many_Things.Core.Services;
using Too_Many_Things.Core.Models;
using Too_Many_Things.Core.ViewModels;
using System.Collections.ObjectModel;
using ReactiveUI;
using Splat;
using DynamicData;
using DynamicData.Binding;
using System.Reactive;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Reactive.Linq;
using Too_Many_Things.Core.DataAccess.Models;

namespace Too_Many_Things.Core.ViewModels
{
    public class PrimaryViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Primary";
        public IScreen HostScreen { get; }
        public ReactiveCommand<Unit, Unit> RefreshCommand { get; }
        private ChecklistDataService _checklistService;

        public PrimaryViewModel(IScreen screen = null, ChecklistDataService checklistService = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            InitializeApp(checklistService);

            // TODO : Cleaner way to automatically refresh when settings is closed or once it's connected?
            RefreshCommand = ReactiveCommand.Create(()=> InitializeApp(null));
        }

        #region Properties
        private string _configurationStatus;
        public string ConfigurationStatus
        {
            get => _configurationStatus;
            set => this.RaiseAndSetIfChanged(ref _configurationStatus, value);
        }

        private ObservableCollection<List> _checklistCache;
        public ObservableCollection<List> ChecklistCache
        {
            get => _checklistCache;
            set => this.RaiseAndSetIfChanged(ref _checklistCache, value);
        }

        #endregion

        #region Asynchronous Tasks & Methods
        /// <summary>
        /// Sets up the primary view to be able to be used if a database
        /// connection has been configured. Otherwise it asks user to configure
        /// it.
        /// </summary>
        private void InitializeApp(ChecklistDataService checklistService)
        {
            var isConfigured = ConnectionStringManager.ConnectionIsConfigured();

            if (isConfigured)
            {
                // Big guns - Retrieves the service from DI.
                ConfigurationStatus = string.Empty;

                _checklistService = checklistService ?? Locator.Current.GetService<ChecklistDataService>();
                ChecklistCache = RetrieveLocalSource();
            }
            else
            {
                // Small guns
                // Changes configuration Status text to inform user that the
                // database hasn't been initialized.
                ConfigurationStatus = "A database has not been configured. Please configure one in the settings. :)";
            }
        }

        private ObservableCollection<List> RetrieveLocalSource()
        {
            return _checklistService.GetLocal();
        }
        #endregion
    }
}
