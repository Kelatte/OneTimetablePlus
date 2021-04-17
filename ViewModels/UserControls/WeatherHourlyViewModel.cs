using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;

using OneTimetablePlus.Models;

namespace OneTimetablePlus.ViewModels.UserControls
{
    public class WeatherHourlyViewModel : ViewModelBase
    {
        #region Constructor

        public WeatherHourlyViewModel(WeatherHourlyInfo info)
        {
            RainInfo = $"降水量 {info?.Precip} 降水率 {info?.Pop}";
            TemperatureInfo = $"气温 {info?.Temp}℃";
            Title = info?.FxDate.Hour.ToString() + " 时天气预报";
        }

        #endregion

        #region Public Properties


        public string RainInfo { get; private set; }

        public string TemperatureInfo { get; private set; }

        public string Title { get; private set; }
        #endregion
    }
}
