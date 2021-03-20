using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace OneTimetablePlus.ValueConverters
{
    /// <summary>
    /// 一个基础的转换器 允许直接的XAML的使用
    /// </summary>
    /// <typeparam name="T">转换器的类型</typeparam>
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
        where T : class, new()
    {
        #region 私有成员

        /// <summary>
        /// 该转换器的单例
        /// </summary>
        private static T _mConverter = null;

        #endregion

        #region Markup Extension Methods
        /// <summary>
        /// 提供一个转换器的静态实例
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _mConverter ??= new T();
        }
        #endregion

        #region 转换器的方法

        /// <summary>
        /// 转换方法
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// 转换一个值为原来的类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        #endregion
    }
}
