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

        #region Reactive Commands & Interactions
        public ReactiveCommand<Unit, Unit> RefreshCommand { get; }
        public ReactiveCommand<Unit, Unit> NewDefaultChecklistCommand { get; }
        public ReactiveCommand<Unit, Unit> EnableRenamingCommand { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> OpenList { get; }
        public ReactiveCommand<Unit, Unit> ConfirmRenameCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelRenameCommand { get; }
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

            // Renamechecklist contextMenu command: TODO
            OpenList = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.Navigate.Execute(new EntryViewModel(SelectedList, HostScreen, _checklistService)));

            var renameCanExecute = this.WhenAnyValue(
                x => x.RenameListInput,
                (input) => input.Length > 0);
            // Change button template style so it looks obvious to user that
            // they can click a button. input.Length > 0 <- true

            // TODO : Make more elegant
            EnableRenamingCommand = ReactiveCommand.Create(() => EnableRenaming());
            ConfirmRenameCommand = ReactiveCommand.CreateFromTask(() => RenameListAsync(), renameCanExecute);
            CancelRenameCommand = ReactiveCommand.Create(() => CancelRename());
        }

        #region Properties
        [Reactive]
        public bool IsConfigured { get; set; }
        [Reactive]
        public string ConfigurationStatus { get; set; }
        [Reactive]
        public ObservableCollection<List> ChecklistCache { get; set; }
        [Reactive]
        public List SelectedList { get; set; }
        [Reactive]
        public string RenameListInput { get; set; } = string.Empty;
        // Quick hacks
        [Reactive]
        public bool IsRenaming { get; set; } = false;
        [Reactive]
        public double GridOppacity { get; set; } = 1;
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

        // Can make this a lot more elegant by using enum property. : TODO
        private void EnableRenaming()
        {
            RenameListInput = "";

            IsRenaming = true;
            GridOppacity = 0.3;
        }

        public async Task RenameListAsync()
        {
            // Ra-naming the checklist.
            await _checklistService.UpdateChecklistNameAsync(SelectedList, RenameListInput);

            IsRenaming = false;
            GridOppacity = 1.0;
        }

        public void CancelRename()
        {
            IsRenaming = false;
            GridOppacity = 1.0;
        }

        private ObservableCollection<List> RetrieveLocalSource()
        {
            return _checklistService.GetLocal();
        }
        #endregion
    }
}
