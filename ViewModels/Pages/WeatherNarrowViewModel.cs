using System;
using System.Collections.Generic;
using System.Text;
using OneTimetablePlus.ViewModels.Application;
using OneTimetablePlus.ViewModels.UserControls;
using OneTimetablePlus.Services;
using OneTimetablePlus.Models;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;


namespace OneTimetablePlus.ViewModels.Pages
{
    public class WeatherNarrowViewModel : ViewModelBase
    {
        #region Constructor

        public WeatherNarrowViewModel(ApplicationViewModel applicationViewModel, IWeatherDataProvider weatherProvider)
        {
            application = applicationViewModel;
            weather = weatherProvider;

            BackCommand = new RelayCommand(Back);
        }

        public void InitListener()
        {
            weather.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(weather.Weather7d))
                {
                    RaisePropertyChanged(nameof(ItemViewModels));
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

        public List<WeatherDailyViewModel> ItemViewModels => GetWeatherDailyViewModels();

        public RelayCommand BackCommand { get; set; }
        #endregion

        #region Private Methods
        private List<WeatherDailyViewModel> GetWeatherDailyViewModels()
        {
            List<WeatherDailyViewModel> vms = new List<WeatherDailyViewModel>();
            foreach (WeatherDailyInfo daily in weather.Weather7d)
            {
                vms.Add(new WeatherDailyViewModel(daily));
            }
            return vms;
        }

        private void Back()
        {
            application.GotoMainPage(ApplicationPage.DayCoursePresent);
        }
        private List<WeatherDailyViewModel> WeatherDailyDemo()
        {
            WeatherDailyInfo dailyInfo = new WeatherDailyInfo
            {
                TempMax = 10,
                TempMin = 5,
                TextDay = "晴",
                TextNight = "小雨",
                IconDay = 100,
                IconNight = 150,
            };
            WeatherDailyViewModel vm = new WeatherDailyViewModel(dailyInfo);

            List<WeatherDailyViewModel> vms = new List<WeatherDailyViewModel>();
            vms.Add(vm);
            vms.Add(vm);
            vms.Add(vm);
            vms.Add(vm);

            return vms;
        }
        #endregion
    }

}
