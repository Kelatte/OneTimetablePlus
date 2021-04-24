using System;
using System.Collections.Generic;
using System.Text;
using OneTimetablePlus.ViewModels.Application;
using OneTimetablePlus.ViewModels.UserControls;
using OneTimetablePlus.Services;
using OneTimetablePlus.Models;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;


namespace OneTimetablePlus.ViewModels.Pages
{
    public class WeatherHourlyViewModel : ViewModelBase
    {
        #region Constructor

        public WeatherHourlyViewModel(ApplicationViewModel applicationViewModel, IWeatherDataProvider weatherProvider)
        {
            application = applicationViewModel;
            weather = weatherProvider;

            BackCommand = new RelayCommand(Back);
            DailyCommand = new RelayCommand(GotoDaily);
        }

        public void InitListener()
        {
            weather.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(weather.Weather7d))
                {
                    RaisePropertyChanged(nameof(ItemViewModels));
                }
                else if(e.PropertyName == nameof(weather.CityName))
                {
                    RaisePropertyChanged(nameof(LocationText));

                }
            };
        }

        #endregion

        #region Private Members
        private readonly IDataProvider data;

        private readonly IWeatherDataProvider weather;

        private readonly ApplicationViewModel application;
        #endregion

        #region Public Properties

        public List<WeatherHourlyItemViewModel> ItemViewModels => GetWeatherHourlyViewModels();

        public RelayCommand BackCommand { get; set; }

        public RelayCommand DailyCommand { get; set; }

        public string LocationText => "地点:" + weather.CityName;

        #endregion

        #region Private Methods

        private List<WeatherHourlyItemViewModel> GetWeatherHourlyViewModels()
        {
            List<WeatherHourlyItemViewModel> vms = new List<WeatherHourlyItemViewModel>();
            foreach (WeatherHourlyInfo hourly in weather.Weather24h)
            {
                if (hourly.FxTime.Hour >= 23 || hourly.FxTime.Hour <= 6)
                    continue;
                vms.Add(new WeatherHourlyItemViewModel(hourly));
            }
            return vms;
        }

        private void Back()
        {
            Console.Write("Back called");
            application.GotoMainPage(ApplicationPage.DayCoursePresent);
        }

        private void GotoDaily()
        {
            Debug.Print("GotoDaily called");
            application.GotoMainPage(ApplicationPage.WeatherDaily);
        }

        
        private List<WeatherHourlyItemViewModel> WeatherHourlyDemo()
        {
            WeatherHourlyInfo dailyInfo = new WeatherHourlyInfo
            {
                
            };
            WeatherHourlyItemViewModel vm = new WeatherHourlyItemViewModel(dailyInfo);

            List<WeatherHourlyItemViewModel> vms = new List<WeatherHourlyItemViewModel>();
            vms.Add(vm);
            vms.Add(vm);
            vms.Add(vm);
            vms.Add(vm);

            return vms;
        }
        #endregion
    }

}
