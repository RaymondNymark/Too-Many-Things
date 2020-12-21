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
using ReactiveUI.Fody.Helpers;

namespace Too_Many_Things.Core.ViewModels
{
    public class PrimaryViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Primary";
        public IScreen HostScreen { get; }

        private ChecklistDataService _checklistService;

        #region Reactive Commands
        public ReactiveCommand<Unit, Unit> RefreshCommand { get; }
        public ReactiveCommand<Unit, Unit> NewDefaultChecklistCommand { get; }
        public ReactiveCommand<Unit, Unit> RenameChecklistCommand { get; }
        #endregion

        public PrimaryViewModel(IScreen screen = null, ChecklistDataService checklistService = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            InitializeApp(checklistService);

            // TODO : Cleaner way to automatically refresh when settings is closed or once it's connected?
            RefreshCommand = ReactiveCommand.Create(() => InitializeApp(null));

            // New Default Checklist command:
            var connectSaveCanExecute = this.WhenAnyValue(
                x => x.IsConfigured,
                (flag) => flag == true);
            NewDefaultChecklistCommand = ReactiveCommand.CreateFromTask(() => _checklistService.AddDefaultChecklist(), connectSaveCanExecute);

            // Renamechecklist contextMenu command:
            RenameChecklistCommand = ReactiveCommand.Create(() => Debug.WriteLine("Testing a bunchhj"));
        }

        #region Properties
        [Reactive]
        public bool IsConfigured { get; set; }
        [Reactive]
        public string ConfigurationStatus { get; set; }
        [Reactive]
        public ObservableCollection<List> ChecklistCache { get; set; }
        [Reactive]
        public List SelectedChecklist { get; set; }
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

                // Flips IsConfigured flag.
                IsConfigured = true;
            }
            else
            {
                // Small guns
                // Changes configuration Status text to inform user that the
                // database hasn't been initialized.
                ConfigurationStatus = "A database has not been configured. Please configure one in the settings. :)";

                // Unflips flag.
                IsConfigured = false;
            }
        }

        private ObservableCollection<List> RetrieveLocalSource()
        {
            return _checklistService.GetLocal();
        }
        #endregion
    }
}
