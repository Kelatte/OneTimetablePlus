using System;
using System.Collections.Generic;
using System.Text;

namespace OneTimetablePlus.Models
{
    public class ColorConfig
    {
        /// <summary>
        /// 所选颜色的名称，需要和Enum中对应
        /// </summary>
        public string SelectedColor { get; set; }

        public List<ColorSampleConfig> ColorSamples { get; set; }
    }
}
