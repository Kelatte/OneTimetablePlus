using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using OneTimetablePlus.Services;
using OneTimetablePlus.Helper;
using OneTimetablePlus.Models;
using OneTimetablePlus.ViewModels.Application;

namespace OneTimetablePlus.ViewModels.Windows
{
    public class EditViewModel : ViewModelBase
    {
        #region Private Members

        /// <summary>
        /// 数据提供者
        /// </summary>
        private readonly IDataProvider data;

        private readonly ApplicationViewModel applicationViewModel;

        private readonly IWeatherDataProvider weather;

        #endregion

        #region Public Commmands

        /// <summary>
        /// 关闭前 命令
        /// </summary>
        public RelayCommand ClosingCommand { get; set; }

        /// <summary>
        /// 删除该课 命令
        /// </summary>
        public RelayCommand DeleteCourseCommand { get; set; }

        /// <summary>
        /// 添加该课 命令
        /// </summary>
        public RelayCommand<string> AddCourseCommand { get; set; }

        /// <summary>
        /// 新加课种 命令
        /// </summary>
        public RelayCommand AddCourseSpeciesCommand { get; set; }

        /// <summary>
        /// 删除课种 命令
        /// </summary>
        public RelayCommand<string> DeleteCourseSpeciesCommand { get; set; }

        /// <summary>
        /// 保存 命令
        /// </summary>
        public RelayCommand SaveCommand { get; set; }

        /// <summary>
        /// 重载 命令
        /// </summary>
        public RelayCommand ReloadCommand { get; set; }

        /// <summary>
        /// 新加周表 命令
        /// </summary>
        public RelayCommand<string> AddWeekCourseCommand { get; set; }

        /// <summary>
        /// 删除此周表 命令
        /// </summary>
        public RelayCommand<WeekCourse> DeleteWeekCourseCommand { get; set; }

        /// <summary>
        /// 关闭后 命令
        /// </summary>
        public RelayCommand ClosedCommand { get; set; }

        /// <summary>
        /// 新加循环课表 命令
        /// </summary>
        public RelayCommand AddCirculatingDayCommand { get; set; }

        /// <summary>
        /// 删除该循环课表 命令
        /// </summary>
        public RelayCommand DeleteCirculatingDayCommand { get; set; }

        #endregion

        #region Public Properties

        //TODO: effect 抗锯齿
        public string SelectedColor
        {
            get => data.SelectedColor;
            set => data.ChangeSelectedColor(value);
        }

        public List<string> ColorNames => data.ColorNames;

        public string WeatherCurrentLocation => weather.CityName;

        public string WeatherLocation
        {
            get => data.WeatherForecastLocation;
            set => data.WeatherForecastLocation = value;
        }

        public bool WeatherForecastEnabled
        {
            get => data.WeatherForecastEnabled;
            set => data.WeatherForecastEnabled = value;
        }

        public List<Course> CourseSpecies => data.CourseSpecies;

        //public List<string> WeekCoursesName => data.WeekCoursesName;

        public List<WeekCourse> WeekCourses => data.WeekCourses;

        /// <summary>
        /// ComboBox 选中的周表
        /// </summary>
        public WeekCourse SelectedWeek
        {
            get => data.SelectedWeek;
            set => data.ChangeSelectedWeek(value);
        }

        /// <summary>
        ///  当前周表的所有日课程
        /// </summary>
        public List<DayCourse> DayCourses => data.AllDayCourses;

        /// <summary>
        /// TabControl 选中的日表
        /// </summary>
        public DayCourse SelectedDayCourse
        {
            get => data.SelectedDayCourse;
            set => data.SelectedDayCourse = value;
        }


        /// <summary>
        /// TabControl 中的 ListBox选中的课程
        /// </summary>
        public Course SelectedCourse { get; set; }


        /// <summary>
        /// 新加课种 全名
        /// </summary>
        public string NewCourseSpeciesFullName { get; set; } = "新课种全名";

        /// <summary>
        /// 新加课种 简名
        /// </summary>
        public string NewCourseSpeciesShowName { get; set; } = "新课种简名";

        /// <summary>
        /// 是否开机自启
        /// </summary>
        public bool IsAutoStart
        {
            get => AutoStart.IfAutoStart();
            set => AutoStart.ChangeAutoStart(value);
        }
        #endregion

        #region Constructor

        public EditViewModel(IDataProvider data, ApplicationViewModel applicationViewModel, IWeatherDataProvider weather)
        {
            this.data = data;
            this.applicationViewModel = applicationViewModel;
            this.weather = weather;

            InitializeListener();
            InitializeCommand();
        }

        /// <summary>
        /// 初始化 数据监听者
        /// </summary>
        private void InitializeListener()
        {
            data.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == GetPropertyName(() => data.CourseSpecies))
                {
                    RaisePropertyChanged(() => CourseSpecies);
                }
                else if (e.PropertyName == GetPropertyName(() => data.WeekCourses))
                {
                    RaisePropertyChanged(() => WeekCourses);
                }
                else if (e.PropertyName == GetPropertyName(() => data.AllDayCourses))
                {
                    RaisePropertyChanged(() => DayCourses);
                }
                else if (e.PropertyName == GetPropertyName(() => data.SelectedWeek))
                {
                    RaisePropertyChanged(() => SelectedWeek);
                }
                else if (e.PropertyName == GetPropertyName(() => data.SelectedDayCourse))
                {
                    RaisePropertyChanged(() => SelectedDayCourse);
                }
                else if (e.PropertyName == GetPropertyName(() => data.WeatherForecastEnabled))
                {
                    RaisePropertyChanged(() => WeatherForecastEnabled);
                }
                else if (e.PropertyName == GetPropertyName(() => data.WeatherForecastLocation))
                {
                    RaisePropertyChanged(() => WeatherLocation);
                }
                else if (e.PropertyName == GetPropertyName(() => data.SelectedColor))
                {
                    RaisePropertyChanged(() => SelectedColor);
                }
                else if (e.PropertyName == GetPropertyName(() => data.ColorNames))
                {
                    RaisePropertyChanged(() => ColorNames);
                }
            };

            weather.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == GetPropertyName(() => weather.CityName))
                {
                    RaisePropertyChanged(() => WeatherCurrentLocation);
                }
            };
        }

        /// <summary>
        /// 初始化 窗体命令绑定
        /// </summary>
        private void InitializeCommand()
        {
            ClosingCommand = new RelayCommand(Closing);
            DeleteCourseCommand = new RelayCommand(DeleteCourse, () => SelectedCourse != null);
            AddCourseCommand = new RelayCommand<string>(AddCourse, delegate { return SelectedDayCourse.Courses.Count < 9; });
            AddCourseSpeciesCommand = new RelayCommand(AddCourseSpecies,
                () => !string.IsNullOrEmpty(NewCourseSpeciesShowName) && !string.IsNullOrEmpty(NewCourseSpeciesFullName));
            DeleteCourseSpeciesCommand = new RelayCommand<string>(DeleteCourseSpecies,
                x => CourseSpecies.Count > 1);
            SaveCommand = new RelayCommand(Save);
            ReloadCommand = new RelayCommand(Reload);
            AddWeekCourseCommand = new RelayCommand<string>(AddWeekCourse);
            DeleteWeekCourseCommand = new RelayCommand<WeekCourse>(DeleteWeekCourse,
                x => WeekCourses.Count > 1);
            ClosedCommand = new RelayCommand(Closed);
            AddCirculatingDayCommand = new RelayCommand(AddCirculatingDay);
            DeleteCirculatingDayCommand = new RelayCommand(DeleteCirculatingDay, IsCirculatingDay);
        }

        #endregion

        #region Commmand Methods

        private void Closing()
        {
            Debug.Print("EditWindow Closing Command");

            if (data.IsUnsaved)
            {
                MessageBoxResult result = MessageBox.Show(
                    "你有修改尚未保存，是否要保存？",
                    "是否要保存？",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    Save();
                }
            }
        }
        private void AddCirculatingDay()
        {
            Debug.Print("EditWindow AddCirculatingDay Command");

            data.AddCirculatingDay();
        }

        private void DeleteCirculatingDay()
        {
            Debug.Print("EditWindow DeleteCirculatingDay Command");

            data.DeleteCirculatingDay();
        }

        private bool IsCirculatingDay()
        {
            return data.IsCirculatingDay();
        }
        private void Closed()
        {

            Debug.Print("EditWindow Closed Command");

            applicationViewModel.EditWindowOpened = false;
        }

        /// <summary>
        /// 删除该课
        /// </summary>
        private void DeleteCourse()
        {
            Debug.Print($"Delete course");

            data.DeleteCourse(SelectedCourse);
        }

        /// <summary>
        /// 添加该课
        /// </summary>
        private void AddCourse(string fullName)
        {
            Debug.Print($"AddCourse fullName={fullName}");

            data.AddCourse(fullName);
        }

        /// <summary>
        /// 新加课种
        /// </summary>
        private void AddCourseSpecies()
        {
            Debug.Print($"Add AddCourseSpecies FullName={NewCourseSpeciesFullName} ShowName={NewCourseSpeciesShowName}");

            data.AddCourseSpecies(NewCourseSpeciesFullName, NewCourseSpeciesShowName);
        }

        /// <summary>
        /// 删除课种
        /// </summary>
        /// <param name="courseFullName"></param>
        private void DeleteCourseSpecies(string courseFullName)
        {
            Debug.Print($"Delete Species name={courseFullName}");

            data.DeleteCourseSpecies(courseFullName);
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        private void Save()
        {
            Debug.Print("Save command");

            data.Save();

        }

        /// <summary>
        /// 重载信息
        /// </summary>
        private void Reload()
        {
            Debug.Print("LoadFromFile command");

            data.ReloadFromFile();

        }

        /// <summary>
        /// 新加周表
        /// </summary>
        public void AddWeekCourse(string weekCourseName)
        {
            Debug.Print($"AddWeek command,Name={weekCourseName}");

            data.AddWeek(weekCourseName);
        }

        /// <summary>
        /// 删除周表
        /// </summary>
        public void DeleteWeekCourse(WeekCourse weekCourse)
        {
            Debug.Print("DeleteWeek command");

            data.DeleteWeek(weekCourse);
        }
        #endregion
    }
}
