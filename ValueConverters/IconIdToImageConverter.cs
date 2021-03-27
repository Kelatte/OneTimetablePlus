using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace OneTimetablePlus.ValueConverters
{
    public class IconIdToImageConverter : BaseValueConverter<IconIdToImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Debug.Print("IconId" + value.ToString());
            if (value == null || (int)value == 0)
                return null;
            
            
            return new BitmapImage(new Uri($"/OneTimetablePlus;component/Assets/WeatherIcon/{value}.png", UriKind.Relative));
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
