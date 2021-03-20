using System;
using System.Collections.Generic;
using System.Text;

namespace OneTimetablePlus.Helper
{
    public static class StringFormat
    {
        /// <summary>
        /// <para>循环日名称 或者 日名称 转成 日名称</para>
        /// <para>例 Monday:1 to Monday</para>
        /// <para>例 Monday to Monday</para>
        /// </summary>
        /// <param name="dayName"></param>
        /// <returns></returns>
        public static string ToDayName(string dayName)
        {
            int index = dayName.IndexOf(":", StringComparison.Ordinal);
            if (index != -1)
            {
                dayName = dayName.Substring(0, index);
            }

            return dayName;
        }

        public static int DayNameToDayId(string dayName)
        {
            return (int)Enum.Parse(typeof(DayOfWeek), dayName);
        }
    }
}
