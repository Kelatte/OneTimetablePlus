using GalaSoft.MvvmLight;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using OneTimetablePlus.Views;

namespace OneTimetablePlus.ViewModels.Application
{
    public class ApplicationViewModel : ViewModelBase
    {
        #region Private Members

        private bool mainWindowOpened = false;

        private bool editWindowOpened = false;

        private MainWindow mainWindow;

        private EditWindow editWindow;

        #endregion

        #region Public Properties

        public bool MainWindowOpened
        {
            get => mainWindowOpened;
            set
            {
                if (mainWindowOpened == value)
                    return;
                if (value)
                {
                    //打开
                    //App.Current.MainWindow = mainWindow;
                    mainWindow = new MainWindow();
                    mainWindow.Show();
                }
                else
                {
                    //关掉
                    mainWindow.Close();
                }
                mainWindowOpened = value;
                RaisePropertyChanged();
            }
        }

        public bool EditWindowOpened
        {
            get => editWindowOpened;
            set
            {
                if (editWindowOpened == value)
                    return;
                if (value)
                {
                    //打开
                    //App.Current.MainWindow = mainWindow;
                    editWindow = new EditWindow();
                    editWindow.Show();
                }
                else
                {
                    //关掉
                    editWindow.Close();
                }
                editWindowOpened = value;
                RaisePropertyChanged();

            }
        }

        #endregion

        public void ShutDown()
        {
            MainWindowOpened = false;
            EditWindowOpened = false;
            System.Windows.Application.Current.Shutdown();
            //Application.Current.Shutdown();
        }
    }
}
