using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;

using OneTimetablePlus.Models;

namespace OneTimetablePlus.ViewModels.UserControls
{
    public class WeatherDailyViewModel : ViewModelBase
    {
        #region Constructor

        public WeatherDailyViewModel(WeatherDailyInfo weatherDailyInfo)
        {
            IconDay = weatherDailyInfo.IconDay;
            IconNight = weatherDailyInfo.IconNight;
            TemperatureInfo = $"气温 {weatherDailyInfo?.TempMin} ~ {weatherDailyInfo?.TempMax}℃";

            var weeks = new string[] { "周日", "周一", "周二", "周三", "周四", "周五", "周六"};
            string dayOfWeek = weeks[(int)weatherDailyInfo.FxDate.DayOfWeek];
            //使 thisWeekEnd 所指日期在周日
            DateTime thisWeekEnd = DateTime.Today.AddDays(7 - (int)DateTime.Now.DayOfWeek);
            if (weatherDailyInfo.FxDate > thisWeekEnd)
                dayOfWeek = "下" + dayOfWeek;
            Title = dayOfWeek + "天气预报";
        }

        #endregion
        #region Public Properties

        public int IconDay { get; private set; }
        
        public int IconNight { get; private set; }

        public string TemperatureInfo { get; private set; }

        public string Title { get; private set; }
        #endregion
    }
}
