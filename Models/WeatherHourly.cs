using System;
using System.Collections.Generic;
using System.Text;

namespace OneTimetablePlus.Models
{
    public class WeatherHourlyInfo
    {
        public DateTime FxTime { get; set; }
        public int Temp { get; set; }
        public float Precip { get; set; }
        public int Pop { get; set; }
    }
}
