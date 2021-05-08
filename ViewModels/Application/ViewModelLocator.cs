using CommonServiceLocator;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

using OneTimetablePlus.Services;
using OneTimetablePlus.ViewModels.Pages;
using OneTimetablePlus.ViewModels.Windows;
using OneTimetablePlus.Helper;

namespace OneTimetablePlus.ViewModels.Application
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
            SimpleIoc.Default.Register<IDataProvider, JsonDataProvider>();
            SimpleIoc.Default.Register<IWeatherDataProvider, WeatherDataProvider>();
            SimpleIoc.Default.Register<ApplicationViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<EditViewModel>();
            SimpleIoc.Default.Register<TaskbarIconViewModel>();
            SimpleIoc.Default.Register<DayCoursePresentViewModel>();
            SimpleIoc.Default.Register<WeatherDailyViewModel>();
            SimpleIoc.Default.Register<WeatherHourlyViewModel>();
            SimpleIoc.Default.Register<ColorChange>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public EditViewModel Edit => ServiceLocator.Current.GetInstance<EditViewModel>();

        public TaskbarIconViewModel TaskbarIcon => ServiceLocator.Current.GetInstance<TaskbarIconViewModel>();

        public DayCoursePresentViewModel DayCoursePresent => ServiceLocator.Current.GetInstance<DayCoursePresentViewModel>();

        public WeatherDailyViewModel WeatherDaily => ServiceLocator.Current.GetInstance<WeatherDailyViewModel>();

        public WeatherHourlyViewModel WeatherHourly => ServiceLocator.Current.GetInstance<WeatherHourlyViewModel>();

        public ColorChange ColorChange => ServiceLocator.Current.GetInstance<ColorChange>();

        public static void Cleanup()
        {
           
        }
    }
}