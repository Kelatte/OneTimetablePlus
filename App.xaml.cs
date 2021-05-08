using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using OneTimetablePlus.Views;
using System.Diagnostics;
using Hardcodet.Wpf.TaskbarNotification;
using GalaSoft.MvvmLight.Ioc;
using OneTimetablePlus.ViewModels.Application;
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
        public App()
        {
        }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
#if DEBUG
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Critical;
#endif
            InitColorChange();

            tb = (TaskbarIcon) FindResource("TaskbarIcon");

            applicationViewModel = SimpleIoc.Default.GetInstance<ApplicationViewModel>();
            applicationViewModel.EditWindowOpened = true;
            //applicationViewModel.MainWindowOpened = true;

        }

        private void InitColorChange()
        {
            Collection<ResourceDictionary> resourceDictionaries = base.Resources.MergedDictionaries;
            ColorChange colorChange = SimpleIoc.Default.GetInstance<ColorChange>();
            colorChange.Init(resourceDictionaries[0], resourceDictionaries[1]);
        }
        private void Test()
        {
            //pack://application:,,,/Styles/Colors.xaml

            Collection<ResourceDictionary> resourceDictionaries = base.Resources.MergedDictionaries;
            ResourceDictionary colorDict = resourceDictionaries[1];
            colorDict["Color1"] = (Color)ColorConverter.ConvertFromString("#FF0000");
            //colorDict["BackgroundVeryLightBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"));


            return;
            Uri uri = new Uri("/Styles/Colors.xaml", UriKind.Relative);
            ResourceDictionary skinDict = Application.LoadComponent(uri) as ResourceDictionary;
            skinDict["BackgroundVeryLightBrush"] = new SolidColorBrush(new Color() { R = 255, G = 0, B = 0 });
        }
    }
}
