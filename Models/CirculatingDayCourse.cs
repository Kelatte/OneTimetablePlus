using GalaSoft.MvvmLight;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace OneTimetablePlus.Models
{
    /// <summary>
    /// 循环日课表
    /// </summary>
    public class CirculatingDayCourse : ObservableObject
    {
        /// <summary>
        /// 所有 日课程
        /// </summary>
        public List<DayCourse> DayCourses { get; set; }

        /// <summary>
        /// 循环到的id
        /// </summary>
        public int CirculatingId { get; set; }

        /// <summary>
        /// 循环到 <see cref="CirculatingId"/> 时的日期
        /// </summary>
        public DateTime CirculatingDate { get; set; }

        /// <summary>
        /// 当日名称  如Monday
        /// </summary>
        public string DayName { get; set; }

    }

}
