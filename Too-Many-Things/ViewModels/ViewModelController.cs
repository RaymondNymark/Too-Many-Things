using System;
using System.Collections.Generic;
using System.Text;
using Too_Many_Things.Services;
using Too_Many_Things.Models;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Too_Many_Things.ViewModels
{
    public class ViewModelController : ViewModelBase
    {
        #region Initialization
        private IChecklistService _checklistService;
        private ViewModelBase _selectedViewModel;


        public ViewModelController(IChecklistService checklistService, ViewModelBase selectedViewModel)
        {
            _checklistService = checklistService;
            _selectedViewModel = selectedViewModel;
        }
        #endregion


        #region View Controls
        public ViewModelBase SelectedViewModel
        {
            get => _selectedViewModel;
            set => SetProperty(ref _selectedViewModel, value);
        }
        #endregion

        // Retrieves collection of checklist to bind to local ItemSource.
        public ObservableCollection<Checklist> ItemSource
        {
            get => _checklistService.GetLocalCollectionSource();
        }

        #region Delegate + ICommands
        /// <summary>
        /// Simplistic delegate command implementation
        /// </summary>
        public class DelegateCommand : ICommand
        {
            public Action CommandAction { get; set; }
            public Func<bool> CanExecuteFunc { get; set; }

            public void Execute(object parameter)
            {
                CommandAction();
            }

            public bool CanExecute(object parameter)
            {
                return CanExecuteFunc == null || CanExecuteFunc();
            }

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
        }
        #endregion
    }
}
