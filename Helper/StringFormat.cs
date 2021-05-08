using System;
using System.Collections.Generic;
using System.Text;

namespace OneTimetablePlus.Helper
{
    public static class StringFormat
    {
        /// <summary>
        /// <para>循环日名称 或者 日名称 转成 日名称</para>
        /// <para>例 Monday:A to Monday</para>
        /// <para>例 Monday to Monday</para>
        /// </summary>
        /// <param name="dayName"></param>
        /// <returns></returns>
        public static string DayNameToDayNamePure(string dayName)
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

        public static int DayNameToIndex(string dayName)
        {
            int index = dayName.IndexOf(":", StringComparison.Ordinal);
            if (index == -1)
            {
                return -1;
            }
            char cha = dayName.Substring(index + 1).ToCharArray()[0];
            return cha - 65;
        }
    }
}
