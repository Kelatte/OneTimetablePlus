using System;
using System.Collections.Generic;
using System.Text;

namespace OneTimetablePlus.Models
{
    public class WeatherDailyInfo
    {
        public DateTime FxDate { get; set; }
        public int TempMax { get; set; }
        public int TempMin { get; set; }
        public int IconDay { get; set; }
        public string TextDay { get; set; }
        public int IconNight { get; set; }
        public string TextNight { get; set; }

    }
}
