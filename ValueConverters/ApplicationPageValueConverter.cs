using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using OneTimetablePlus.Models;
using System.Diagnostics;
using OneTimetablePlus.Resources.Pages;

namespace OneTimetablePlus.ValueConverters
{
    class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.DayCoursePresent:
                    return new DayCoursePresentPage();

                case ApplicationPage.WeatherNarrow:
                    return new WeatherNarrowPage();

                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
