using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

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
using OneTimetablePlus.ViewModels.UserControls;
using System.Net.Http;
using System.Windows;

namespace OneTimetablePlus.ViewModels.Pages
{
    public class DayCoursePresentViewModel : ViewModelBase
    {
        #region Constructor

        public DayCoursePresentViewModel(ApplicationViewModel applicationViewModel, IDataProvider dataProvider, IWeatherDataProvider weatherProvider)
        {
            application = applicationViewModel;
            data = dataProvider;
            weather = weatherProvider;

            InitializeListener();

            GoToWeatherPageCommand = new RelayCommand(GoToWeatherPage);

            //TODO: 仅在窗口打开后才执行下面语句
            InitTime();
            if (data.WeatherForecastEnabled)
            {
                InitWeather();
            }
        }
        private void InitializeListener()
        {
            data.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == GetPropertyName(() => data.TodayDayCourse))
                {
                    //Debug.Print("Catch PropertyChanged TodayDayCourse");
                    RaisePropertyChanged(() => TodayDayCourse);
                }
                else if (e.PropertyName == GetPropertyName(() => data.WeatherForecastEnabled))
                {
                    RaisePropertyChanged(() => WeatherVisibility);
                    if (data.WeatherForecastEnabled)
                    {
                        InitWeather();
                    }
                }
            };

            weather.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == GetPropertyName(() => weather.WeatherTomorrow))
                {
                    RaisePropertyChanged(() => WeatherViewModel);
                }
            };
        }

        private async void InitTime()
        {
            int msPast = DateTime.Now.Millisecond + DateTime.Now.Second * 1000;

            await Task.Delay(60 * 1000 - msPast);

            RaisePropertyChanged(() => ShortTime);

            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 1, 0),
                DispatcherPriority.Normal,
                delegate
                {
                    RaisePropertyChanged(() => ShortTime);
                },
                Dispatcher.CurrentDispatcher);

        }

        private async void InitWeather()
        {
            //Debug.Print("InitWeather called");

            try
            {
                await weather.RefreshWeather();

            }
            catch (Exception e)
            {
                MessageBox.Show("请检查您的网络是否通畅\r\n" + e.Message, "刷新天气错误");
            }



        }

        #endregion

        #region Private Members
        private readonly IDataProvider data;

        private readonly IWeatherDataProvider weather;

        private readonly ApplicationViewModel application;
        #endregion

        #region Public Properties

        /// <summary>
        /// 今日课表
        /// </summary>
        public DayCourse TodayDayCourse => data.TodayDayCourse;

        public string ShortTime => DateTime.Now.ToString("HH:mm");

        public WeatherDailyItemViewModel WeatherViewModel
        {
            get
            {
                if (weather.WeatherTomorrow != null)
                {
                    return new WeatherDailyItemViewModel(weather.WeatherTomorrow);
                }
                else
                {
                    return null;
                }
            }
        }

        public string WeatherVisibility => data.WeatherForecastEnabled ? "Visible" : "Collapsed";

        public RelayCommand GoToWeatherPageCommand { get; set; }

        //public List<WeatherHourly>
        #endregion

        #region Private Methods
        
        private void GoToWeatherPage()
        {
            //Debug.Print("GoToWeatherPage Called");
            application.GotoMainPage(ApplicationPage.WeatherDaily);
        }
        #endregion
    }
}
