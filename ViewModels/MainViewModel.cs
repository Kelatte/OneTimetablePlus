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
using OneTimetablePlus.Helper;
using System.Net.Http;
using System.Windows;

namespace OneTimetablePlus.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Constructor

        public MainViewModel(IDataProvider dataProvider)
        {
            data = dataProvider;

           
        }

        #endregion

        #region Private Members

        private readonly IDataProvider data;

        #endregion

        #region Public Proprities

        public ApplicationPage CurrentPage { get; set; } = 0;

        #endregion

    }
}