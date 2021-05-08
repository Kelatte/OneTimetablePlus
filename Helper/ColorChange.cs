using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using OneTimetablePlus.Models;
using System.Collections.ObjectModel;
using OneTimetablePlus.Services;
using System.Linq;

namespace OneTimetablePlus.Helper
{
    public class ColorChange
    {
        private ResourceDictionary colorDictionary;
        private ResourceDictionary brushDictionary;

        public ColorChange()
        {
        }

        public void Init(ResourceDictionary colorDictionary, ResourceDictionary brushDictionary)
        {
            this.colorDictionary = colorDictionary;
            this.brushDictionary = brushDictionary;
        }

        public void FromColors(Color? BackgroundVeryLight,
                                      Color? BackgroundLight,
                                      Color? ForegroundLight,
                                      Color? ForegroundMain,
                                      Color? ForegroundDark)
        {
            throw new Exception();
            /*
            if (BackgroundVeryLight.HasValue)
                colorDictionary["Color1"] = BackgroundVeryLight;
            if (BackgroundLight.HasValue)
                colorDictionary["Color2"] = BackgroundLight;
            if (ForegroundLight.HasValue)
                colorDictionary["Color3"] = ForegroundLight;
            if (ForegroundMain.HasValue)
                colorDictionary["Color4"] = ForegroundMain;
            if (ForegroundDark.HasValue)
                colorDictionary["Color5"] = ForegroundDark;
            */
        }

        public void FromDefaultSample(ColorSample sample)
        {
            //默认配置里的
            Uri uri = new Uri($"/Styles/ColorSamples/{sample}.xaml", UriKind.Relative);
            ResourceDictionary rd = Application.LoadComponent(uri) as ResourceDictionary;

            brushDictionary["BackgroundVeryLightBrush"] = new SolidColorBrush((Color)rd["Color1"]);
            brushDictionary["BackgroundLightBrush"] = new SolidColorBrush((Color)rd["Color2"]);
            brushDictionary["ForegroundLightBrush"] = new SolidColorBrush((Color)rd["Color3"]);
            brushDictionary["ForegroundMainBrush"] = new SolidColorBrush((Color)rd["Color4"]);
            brushDictionary["ForegroundDarkBrush"] = new SolidColorBrush((Color)rd["Color5"]);
            foreach (string key in rd.Keys)
            {
                colorDictionary[key] = rd[key];
            }

        }

        public void FromCustomedSample(ColorSampleConfig config)
        { 
            brushDictionary["BackgroundVeryLightBrush"] = new SolidColorBrush(config.Color1);
            brushDictionary["BackgroundLightBrush"] = new SolidColorBrush(config.Color2);
            brushDictionary["ForegroundLightBrush"] = new SolidColorBrush(config.Color3);
            brushDictionary["ForegroundMainBrush"] = new SolidColorBrush(config.Color4);
            brushDictionary["ForegroundDarkBrush"] = new SolidColorBrush(config.Color5);
            brushDictionary["Color1"] = config.Color1;
            brushDictionary["Color2"] = config.Color2;
            brushDictionary["Color3"] = config.Color3;
            brushDictionary["Color4"] = config.Color4;
            brushDictionary["Color5"] = config.Color5;
        }
    }
}
