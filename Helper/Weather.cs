using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OneTimetablePlus.Helper
{
    class Weather
    {
        //test git : some code
        private const string Key = "368b03f9e5974d50ac89f74fe70e1049";

        private string city;
        private string region;
        private string id;

        public async Task GetLocation()
        {
            HttpClient httpClient = new HttpClient() {Timeout = TimeSpan.FromSeconds(3)};
            string uri = "http://ip-api.com/json/";
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject responseJson = JsonConvert.DeserializeObject(responseBody) as JObject;
            string status = (string) responseJson?["status"];
            if (status != "success")
            {
                Exception e = new Exception($"城市位置获取错误, status = {status}");
                throw e;
            }
            city = responseJson?["city"]?.ToString().Replace(" ", "");
            region = responseJson?["regionName"]?.ToString().Replace(" ", "");
        }

        public async Task GetLocationId()
        {
            if (city == null || region == null)
                await GetLocation();
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

        public async Task<List<WeatherDayInfo>> Get3dWeather()
        {
            if (id == null)
                await GetLocationId();
            HttpClientHandler handler = new HttpClientHandler() {AutomaticDecompression = DecompressionMethods.GZip};
            HttpClient httpClient = new HttpClient(handler) {Timeout = TimeSpan.FromSeconds(3)};

            string uri = "https://devapi.qweather.com/v7/weather/3d?location={0}&key={1}";
            uri = string.Format(uri, id, Key);
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
            List<WeatherDayInfo> result = new List<WeatherDayInfo>();
            if (days == null)
            {
                Exception e = new Exception("天气预报获取错误 \r\n uri = {uri} \r\n " + nameof(days) + " == null");
                throw e;
            }
            foreach (JToken day in days)
            {
                WeatherDayInfo info = new WeatherDayInfo
                {
                    TempMax = (string) day["tempMax"],
                    TempMin = (string) day["tempMin"],
                    FxDate = (DateTime) day["fxDate"],
                    IconDay = (string) day["iconDay"],
                    TextDay = (string) day["textDay"],
                    IconNight = (string) day["iconNight"],
                    TextNight = (string) day["textNight"]
                };
                result.Add(info);
            }

            return result;


        }

    }
    public class WeatherDayInfo
    {
        public DateTime FxDate { get; set; }
        public string TempMax { get; set; }
        public string TempMin { get; set; }
        public string IconDay { get; set; }
        public string TextDay { get; set; }
        public string IconNight { get; set; }
        public string TextNight { get; set; }

    }
    public class WeahterHourlyInfo
    {
        public DateTime FxDate { get; set; }
        public string Temp { get; set; }
        public string Text { get; set; }
    }
}
