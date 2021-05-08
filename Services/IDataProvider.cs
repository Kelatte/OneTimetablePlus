using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using GalaSoft.MvvmLight;
using OneTimetablePlus.Models;

namespace OneTimetablePlus.Services
{
    public interface IDataProvider
    {
        List<string> ColorNames { get; }

        string SelectedColor { get; }

        void ChangeSelectedColor(string name);

        void DeleteColor(string name);

        void AddColor(ColorSampleConfig config);

        event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 是否改了且未保存
        /// </summary>
        bool IsUnsaved { get; set; }

        /// <summary>
        /// 删除该课
        /// </summary>
        void DeleteCourse(Course selectedCourse);

        /// <summary>
        /// 添加该课
        /// </summary>
        void AddCourse(string fullName);

        /// <summary>
        /// 删除课种
        /// </summary>
        /// <param name="courseName"></param>
        void DeleteCourseSpecies(string courseName);

        /// <summary>
        /// 添加课种
        /// </summary>
        /// <param name="fullName">课种全名</param>
        /// <param name="showName">课种显示名</param>
        void AddCourseSpecies(string fullName, string showName);

        /// <summary>
        /// 保存 至文件
        /// </summary>
        void Save();

        /// <summary>
        /// 重载 从文件
        /// </summary>
        void ReloadFromFile();

        /// <summary>
        /// 添加周表
        /// </summary>
        /// <param name="addName"></param>
        void AddWeek(string addName);

        /// <summary>
        /// 删除周表
        /// </summary>
        void DeleteWeek(WeekCourse weekCourse);


        
        /// <summary>
        /// 选中的周表
        /// </summary>
        WeekCourse SelectedWeek { get; }

        /// <summary>
        /// 获取今天的 DayCourses
        /// </summary>
        /// <returns></returns>
        DayCourse TodayDayCourse { get; }

        /// <summary>
        /// 课程种类
        /// </summary>
        List<Course> CourseSpecies { get; }

        /// <summary>
        /// 获取选中周表的 所有日课程
        /// </summary>
        /// <returns></returns>
        List<DayCourse> AllDayCourses { get; }

        /// <summary>
        /// 添加循环日表
        /// </summary>
        void AddCirculatingDay();

        /// <summary>
        /// 删除循环日表
        /// </summary>
        void DeleteCirculatingDay();

        /// <summary>
        /// 是否是循环课表
        /// </summary>
        bool IsCirculatingDay();

        /// <summary>
        /// 在当前周表中 选中的日课程(可能为循环课表中的一天)
        /// </summary>
        DayCourse SelectedDayCourse { get; set; }

        /// <summary>
        /// 修改选中的周id
        /// </summary>
        /// <param name="selectedWeek"></param>
        void ChangeSelectedWeek(WeekCourse selectedWeek);

        /// <summary>
        /// 所有周表
        /// </summary>
        List<WeekCourse> WeekCourses{ get; }

        /// <summary>
        /// 是否启用天气预报功能
        /// </summary>
        bool WeatherForecastEnabled { get; set; }

        /// <summary>
        /// 天气预报默认地点 CityName
        /// </summary>
        string WeatherForecastLocation { get; set; }
    }
}
