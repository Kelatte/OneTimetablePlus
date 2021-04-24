using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;

using OneTimetablePlus.Models;

namespace OneTimetablePlus.ViewModels.UserControls
{
    public class WeatherHourlyItemViewModel : ViewModelBase
    {
        #region Constructor

        public WeatherHourlyItemViewModel(WeatherHourlyInfo info)
        {
            //RainInfo = $"降水 {info?.Precip}mm 率 {info?.Pop}";
            //TemperatureInfo = $"气温 {info?.Temp}℃";
            Title = $"{info?.FxTime.Hour}时 气温 {info?.Temp}℃";
            if (info?.Precip != 0 || info?.Pop !=0)
            {
                Title += $"\r\n降水 {info?.Precip} 率 {info?.Pop}";
            }
        }

        #endregion

        #region Public Properties


        public string RainInfo { get; private set; }

        public string TemperatureInfo { get; private set; }

        public string Title { get; private set; }
        #endregion
    }
}
