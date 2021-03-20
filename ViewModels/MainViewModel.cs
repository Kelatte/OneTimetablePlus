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

            //TODO: ���ڴ��ڴ򿪺��ִ���������
            InitTime();
            if (data.WeatherForecastEnabled)
            {
                InitWeather();
            }
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
            Debug.Print("InitWeather called");

            Weather weather = new Weather();
            try
            {
                List<WeatherDayInfo> infos = await weather.Get3dWeather();
                WeatherInfo = infos[1];
                RaisePropertyChanged(() => WeatherInfo);
                RaisePropertyChanged(() => TemperatureInfo);
            }
            catch(Exception e)
            {
                MessageBox.Show("�������������Ƿ�ͨ��\r\n" + e.Message, "�������");
            }
            
            //ֻ��ȡ���������

        }
        #endregion

        #region Private Members
        private readonly IDataProvider data;
        #endregion

        #region Public Proprities

        /// <summary>
        /// ���տα�
        /// </summary>
        public DayCourse TodayDayCourse => data.TodayDayCourse;

        public string ShortTime => DateTime.Now.ToString("HH:mm");

        public WeatherDayInfo WeatherInfo { get; set; }

        public string TemperatureInfo => $"���� {WeatherInfo?.TempMin} ~ {WeatherInfo?.TempMax}��";

        public string WeatherVisibility => data.WeatherForecastEnabled ? "Visible" : "Collapsed";

        public List<WeatherHourly>
        #endregion

    }
}