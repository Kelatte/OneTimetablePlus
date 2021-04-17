using System;
using System.Collections.Generic;
using System.Text;

namespace OneTimetablePlus.Models
{
    public class WeatherHourlyInfo
    {
        public DateTime FxDate { get; set; }
        public string Temp { get; set; }
        public int Precip { get; set; }
        public int Pop { get; set; }
    }
}
