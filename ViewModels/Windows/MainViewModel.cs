using GalaSoft.MvvmLight;
using OneTimetablePlus.Services;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;
using OneTimetablePlus.Models;
using OneTimetablePlus.ViewModels.Application;
using System.Net.Http;
using System.Windows;

namespace OneTimetablePlus.ViewModels.Windows
{
    public class MainViewModel : ViewModelBase
    {
        #region Constructor

        public MainViewModel(ApplicationViewModel applicationViewModel, IDataProvider dataProvider)
        {
            application = applicationViewModel;
            data = dataProvider;

            application.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == GetPropertyName(() => application.MainCurrentPage))
                {
                    RaisePropertyChanged(nameof(CurrentPage));
                }
            };
        }


        #endregion

        #region Private Members

        private readonly IDataProvider data;

        private readonly ApplicationViewModel application;
        #endregion

        #region Public Properties

        public ApplicationPage CurrentPage => application.MainCurrentPage;

        #endregion

    }
}