using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;

using OneTimetablePlus.Services;
using OneTimetablePlus.Views;

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace OneTimetablePlus.ViewModels
{
    public class TaskbarIconViewModel : ViewModelBase
    {

        #region Private Menbers

        private readonly ApplicationViewModel applicationViewModel;

        #endregion

        #region Public Properties

        public bool MainWindowOpened
        {
            get => applicationViewModel.MainWindowOpened;
            set => applicationViewModel.MainWindowOpened = value;
        }

        public bool EditWindowOpened
        {
            get => applicationViewModel.EditWindowOpened;
            set => applicationViewModel.EditWindowOpened = value;
        }
        
        public RelayCommand ShutDownCommand { get; set; }

        #endregion

        #region Constructor

        public TaskbarIconViewModel(ApplicationViewModel applicationViewModel)
        {
            this.applicationViewModel = applicationViewModel;

            ShutDownCommand = new RelayCommand(applicationViewModel.ShutDown);

            applicationViewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == GetPropertyName(() => applicationViewModel.MainWindowOpened))
                {
                    RaisePropertyChanged(() => MainWindowOpened);
                }else if (e.PropertyName == GetPropertyName(() => applicationViewModel.EditWindowOpened))
                {
                    RaisePropertyChanged(() => EditWindowOpened);
                }
                
            };
        }

        #endregion


    }
}
