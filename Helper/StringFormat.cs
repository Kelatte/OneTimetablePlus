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

        /// <summary>
        /// 返回星期号对应的序号（周日开始，0-based）
        /// </summary>
        /// <example>
        /// Sunday => 0
        /// Monday => 1
        /// </example>
        /// <param name="dayName"></param>
        /// <returns></returns>
        public static int DayNamePureToDayId(string dayName)
        {
            return (int)Enum.Parse(typeof(DayOfWeek), dayName);
        }

        /// <summary>
        /// 循环课表名称 转为 循环课表序号(0-based)
        /// </summary>
        /// <param name="dayName"></param>
        /// <returns></returns>
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
