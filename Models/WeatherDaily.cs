using System;
using System.Collections.Generic;
using System.Text;

namespace OneTimetablePlus.Models
{
    public class WeatherDailyInfo
    {
        public DateTime FxDate { get; set; }
        public string TempMax { get; set; }
        public string TempMin { get; set; }
        public string IconDay { get; set; }
        public string TextDay { get; set; }
        public string IconNight { get; set; }
        public string TextNight { get; set; }

    }
}
