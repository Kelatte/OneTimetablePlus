using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using OneTimetablePlus.Views;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Diagnostics;
using OneTimetablePlus.Services;
using Hardcodet.Wpf.TaskbarNotification;
using OneTimetablePlus.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using OneTimetablePlus.Helper;

namespace OneTimetablePlus
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon tb;
        private ApplicationViewModel applicationViewModel;
        private void Application_Startup(object sender, StartupEventArgs e)
        {

            //Test();
            //return;
#if DEBUG
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Critical;
#endif

            tb = (TaskbarIcon) FindResource("TaskbarIcon");

            applicationViewModel = SimpleIoc.Default.GetInstance<ApplicationViewModel>();
            //applicationViewModel.EditWindowOpened = true;
            applicationViewModel.MainWindowOpened = true;
        }

        private void Test()
        {
            Debug.WriteLine(SystemParameters.PrimaryScreenHeight);
        }
    }
}
