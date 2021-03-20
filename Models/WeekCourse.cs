using GalaSoft.MvvmLight;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace OneTimetablePlus.Models
{
    /// <summary>
    /// 一周的所有日课程
    /// </summary>
    public class WeekCourse : ObservableObject
    {
        /// <summary>
        /// 一周的每天 日课程
        /// </summary>
        public List<DayCourse> DayCourses{ get; set; }

        /// <summary>
        /// 循环课表
        /// </summary>
        public List<CirculatingDayCourse> CirculatingCourses { get; set; }

        /// <summary>
        /// 这周的名称 如:默认，A，B，C
        /// </summary>
        public string WeekName{ get; set; }

    }
}
