using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OneTimetablePlus.Models;
using System.ComponentModel;

namespace OneTimetablePlus.Services
{
    public interface IWeatherDataProvider
    {
        event PropertyChangedEventHandler PropertyChanged;

        public List<WeatherDailyInfo> Weather7d { get; }

        public List<WeatherHourlyInfo> Weather24h { get; }

        public WeatherDailyInfo WeatherTomorrow { get; }

        public string CityName { get; }

        public Task RefreshWeather();

        public Task RefreshLocation();


    }

}
