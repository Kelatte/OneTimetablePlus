using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using GalaSoft.MvvmLight;

namespace OneTimetablePlus.Models
{
    public class WholeData : ObservableObject
    {

        #region Public Properties

        /// <summary>
        /// 所有周表
        /// </summary>
        public List<WeekCourse> WeekCourses { get; set; }

        /// <summary>
        /// 所有课的类别
        /// </summary>
        public List<Course> CourseSpecies { get; set; }

        /// <summary>
        /// 数据简介
        /// </summary>
        public DataDescription DataDescription{ get; set; }

        /// <summary>
        /// 选中的周表id
        /// </summary>
        public int SelectedWeekId { get; set; }

        /// <summary>
        /// 是否启用天气预报
        /// </summary>
        public bool WeatherForecastEnabled { get; set; }
        #endregion
    }

    public class DataDescription
    {
        /// <summary>
        /// 编辑者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 有关
        /// </summary>
        public string About { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateDate { get; set; }
    }
}
