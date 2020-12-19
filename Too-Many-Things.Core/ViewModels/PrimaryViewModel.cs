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

namespace Too_Many_Things.Core.ViewModels
{
    public class PrimaryViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Primary";
        public IScreen HostScreen { get; }
        public ReactiveCommand<Unit, Unit> RefreshCommand { get; }

        public PrimaryViewModel(IScreen screen = null, IChecklistService checklistService = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            RefreshCommand = ReactiveCommand.Create(()=> InitializeApp());

            InitializeApp();
        }

        #region Properties
        private string _configurationStatus;
        public string ConfigurationStatus
        {
            get => _configurationStatus;
            set => this.RaiseAndSetIfChanged(ref _configurationStatus, value);
        }

        #endregion

        #region Asynchronous Tasks & Methods
        /// <summary>
        /// Sets up the primary view to be able to be used if a database
        /// connection has been configured. Otherwise it asks user to configure
        /// it.
        /// </summary>
        private void InitializeApp()
        {
            var isConfigured = ConnectionStringManager.ConnectionIsConfigured();

            if (isConfigured)
            {
                // Big guns
                // Bring DB data into a collection<t> from service and do things. 
            }
            else
            {
                // Small guns
                // Changes configuration Status text to inform user that the
                // database hasn't been initialized.
                ConfigurationStatus = "A database has not been configured. Please configure one in the settings. :)";
            }
        }
        #endregion
    }
}
