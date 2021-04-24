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
    public class WeatherDailyViewModel : ViewModelBase
    {
        #region Constructor

        public WeatherDailyViewModel(ApplicationViewModel applicationViewModel, IWeatherDataProvider weatherProvider)
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

        public List<WeatherDailyItemViewModel> ItemViewModels => GetWeatherDailyViewModels();

        public RelayCommand BackCommand { get; set; }

        public string LocationText => "地点:" + weather.CityName;

        #endregion

        #region Private Methods
        private List<WeatherDailyItemViewModel> GetWeatherDailyViewModels()
        {
            List<WeatherDailyItemViewModel> vms = new List<WeatherDailyItemViewModel>();
            foreach (WeatherDailyInfo daily in weather.Weather7d)
            {
                vms.Add(new WeatherDailyItemViewModel(daily));
            }
            return vms;
        }

        private void Back()
        {
            application.GotoMainPage(ApplicationPage.DayCoursePresent);
        }
        private List<WeatherDailyItemViewModel> WeatherDailyDemo()
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
            WeatherDailyItemViewModel vm = new WeatherDailyItemViewModel(dailyInfo);

            List<WeatherDailyItemViewModel> vms = new List<WeatherDailyItemViewModel>();
            vms.Add(vm);
            vms.Add(vm);
            vms.Add(vm);
            vms.Add(vm);

            return vms;
        }
        #endregion
    }

}
