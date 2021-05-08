using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace OneTimetablePlus.Models
{
    /// <summary>
    /// 一个颜色配置的数据（1一个名称、5个颜色）
    /// </summary>
    public class ColorSampleConfig
    {
        public string Name { get; set; }
        public Color Color1 { get; set; }
        public Color Color2 { get; set; }
        public Color Color3 { get; set; }
        public Color Color4 { get; set; }
        public Color Color5 { get; set; }
    }
}
