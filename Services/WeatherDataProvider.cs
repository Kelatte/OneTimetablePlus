using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneTimetablePlus.Models;
using System.Diagnostics;

namespace OneTimetablePlus.Services
{
    class WeatherDataProvider : ObservableObject, IWeatherDataProvider
    {
        #region Constructers

        public WeatherDataProvider(IDataProvider dataProvider)
        {
            //初始化city之类的
#if DEBUG
            //模拟在线推断好了地址
            //city = "长兴";
            //id = "101210205";
#endif

            data = dataProvider;

            if (!string.IsNullOrWhiteSpace(data.WeatherForecastLocation))
            {
                city = data.WeatherForecastLocation;
                RaisePropertyChanged(() => CityName);
            }

            data.PropertyChanged += async (sender, e) =>
            {
                if (e.PropertyName == GetPropertyName(() => data.WeatherForecastLocation)
                && !string.IsNullOrWhiteSpace(data.WeatherForecastLocation))
                {
                    city = data.WeatherForecastLocation;
                    RaisePropertyChanged(() => CityName);

                    Debug.Print($"data.WeatherForecastLocation changed : {data.WeatherForecastLocation}");

                    await RefreshLocation();
                    await RefreshWeather();
                }
            };


        }
        #endregion

        #region Private Members

        private const string Key = "368b03f9e5974d50ac89f74fe70e1049";

        private readonly IDataProvider data;

        /// <summary>
        /// 本地存储的默认地址 或 者在线通过ip推断的地址
        /// </summary>
        private string city;

        private string region;
        private string id;

        private List<WeatherDailyInfo> weather3d;
        private List<WeatherDailyInfo> weather7d;
        private List<WeatherHourlyInfo> weather24h;

        #endregion

        #region Public Properties

        public WeatherDailyInfo WeatherTomorrow => weather7d?[1];
        public List<WeatherDailyInfo> Weather7d => weather7d;
        public List<WeatherHourlyInfo> Weather24h => weather24h;
        public string CityName => city;

        #endregion

        #region Public Methods

        public async Task RefreshWeather()
        {
            if (id == null)
                await RefreshLocation();

            //TODO: 暂时全部刷新，后来再分3d和7d刷新
            //weather3d = await Get3dWeather();
            weather7d = await Get7dWeather();
            weather24h = await GetHoursWeather();

            RaisePropertyChanged(() => WeatherTomorrow);
            RaisePropertyChanged(() => Weather7d);
            RaisePropertyChanged(() => Weather24h);
        }

        public async Task RefreshLocation()
        {
            await RefreshLocationId();
        }
        #endregion

        #region Private Methods

        private async Task RefreshCityRegion()
        {
            HttpClient httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(3) };
            string uri = "http://ip-api.com/json/";
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject responseJson = JsonConvert.DeserializeObject(responseBody) as JObject;
            string status = (string)responseJson?["status"];
            if (status != "success")
            {
                Exception e = new Exception($"城市位置获取错误, status = {status}");
                throw e;
            }
            city = responseJson?["city"]?.ToString().Replace(" ", "");
            region = responseJson?["regionName"]?.ToString().Replace(" ", "");

            RaisePropertyChanged(nameof(CityName));
        }

        private async Task RefreshLocationId()
        {
            if (string.IsNullOrWhiteSpace(city) && string.IsNullOrWhiteSpace(region))
                await RefreshCityRegion();
            HttpClientHandler handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            HttpClient httpClient = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(3) };
            string uri = "https://geoapi.qweather.com/v2/city/lookup?key={2}&location={0}&range=cn&adm={1}";
            uri = string.Format(uri, city, region, Key);
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject responseJson = JsonConvert.DeserializeObject(responseBody) as JObject;
            int code = (int)responseJson?["code"];
            if (code != 200)
            {
                Exception e = new Exception($"城市位置id获取错误 \r\n uri = {uri} \r\n code = { code }");
                throw e;
            }
            id = responseJson?["location"]?[0]?["id"]?.ToString();

        }

        private async Task<List<WeatherDailyInfo>> GetDaysWeather(int dayNumber)
        {
            if (dayNumber != 3 && dayNumber != 7)
                return null;
            if (id == null)
                await RefreshLocationId();
            string uri = "https://devapi.qweather.com/v7/weather/{2}d?location={0}&key={1}";
            uri = string.Format(uri, id, Key, dayNumber.ToString());

            HttpClientHandler handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            HttpClient httpClient = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(3) };
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject responseJson = JsonConvert.DeserializeObject(responseBody) as JObject;
            int code = (int)responseJson?["code"];
            if (code != 200)
            {
                Exception e = new Exception($"天气预报获取错误 \r\n uri = {uri} \r\n code = {code}");
                throw e;
            }
            JArray days = responseJson?["daily"] as JArray;
            List<WeatherDailyInfo> result = new List<WeatherDailyInfo>();
            if (days == null)
            {
                Exception e = new Exception("天气预报获取错误 \r\n uri = {uri} \r\n " + nameof(days) + " == null");
                throw e;
            }
            foreach (JToken day in days)
            {
                WeatherDailyInfo info = new WeatherDailyInfo
                {
                    TempMax = (int)day["tempMax"],
                    TempMin = (int)day["tempMin"],
                    FxDate = (DateTime)day["fxDate"],
                    IconDay = (int)day["iconDay"],
                    TextDay = (string)day["textDay"],
                    IconNight = (int)day["iconNight"],
                    TextNight = (string)day["textNight"]
                };
                result.Add(info);
            }

            return result;


        }

        private async Task<List<WeatherHourlyInfo>> GetHoursWeather()
        {
            if (id == null)
                await RefreshLocationId();
            string uri = "https://devapi.qweather.com/v7/weather/24h?location={0}&key={1}";
            uri = string.Format(uri, id, Key);

            HttpClientHandler handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            HttpClient httpClient = new HttpClient(handler) { Timeout = TimeSpan.FromSeconds(3) };
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject responseJson = JsonConvert.DeserializeObject(responseBody) as JObject;
            int code = (int)responseJson?["code"];
            if (code != 200)
            {
                Exception e = new Exception($"天气预报获取错误 \r\n uri = {uri} \r\n code = {code}");
                throw e;
            }

            JArray hours = responseJson?["hourly"] as JArray;
            List<WeatherHourlyInfo> result = new List<WeatherHourlyInfo>();
            if (hours == null)
            {
                Exception e = new Exception("天气预报获取错误 \r\n uri = {uri} \r\n " + nameof(hours) + " == null");
                throw e;
            }
            foreach (JToken hour in hours)
            {
                WeatherHourlyInfo info = new WeatherHourlyInfo
                {
                    Temp = (int)hour["temp"],
                    Precip = (float)hour["precip"],
                    Pop = (int)hour["pop"],
                    FxTime = (DateTime)hour["fxTime"],
                };
                result.Add(info);
            }

            return result;

        }


        private async Task<List<WeatherDailyInfo>> Get3dWeather()
        {
            return await GetDaysWeather(3);
        }

        private async Task<List<WeatherDailyInfo>> Get7dWeather()
        {
            return await GetDaysWeather(7);
        }
        #endregion

    }
}
