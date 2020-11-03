using System;
using System.Collections.Generic;
using System.Text;
using Too_Many_Things.Services;
using Too_Many_Things.Models;


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


    }
}
