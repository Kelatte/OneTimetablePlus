using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OneTimetablePlus.Models
{
    /// <summary>
    /// 一日的所有课程
    /// </summary>
    public class DayCourse
    {
        /// <summary>
        /// 一日的课程
        /// </summary>
        public List<Course> Courses { get; set; }

        /// <summary>
        /// 当日名称
        /// <para>做单独日课表时 为 Monday</para>
        /// <para>做循环课表时 为 Monday:A</para>
        /// </summary>
        public string DayName { get; set; }

        //课程时间

    }




}
