﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.IO;
using OneTimetablePlus.Models;
using GalaSoft.MvvmLight;
using OneTimetablePlus.Helper;
using System.Windows.Media;

namespace OneTimetablePlus.Services
{
    public class JsonDataProvider : ObservableObject, IDataProvider
    {
        #region Private Members

        private ColorChange colorChange;

        /// <summary>
        /// JSON文件的完整路径
        /// </summary>
        private readonly string dataPath;

        private bool isUnsaved;
        private DayCourse selectedDayCourse;

        /// <summary>
        /// JSON全部数据
        /// </summary>
        private WholeData WholeData { get; set; }

        #endregion

        #region Public Properties

        //此处直接在属性的set事件中进行RaisePropertyChanged，因为几乎每个操作都会导致改变，都将Raise聚到这里可以减少代码冗余
        public bool IsUnsaved
        {
            get => isUnsaved;
            set
            {
                if (value != isUnsaved)
                    Set(ref isUnsaved, value);
            }
        }

        //此处直接在属性的set事件中进行RaisePropertyChanged，因为View中可以对该属性直接修改而不经过Mothod。但也因此不可以在Method中多次修改
        public DayCourse SelectedDayCourse { get => selectedDayCourse; set => Set(ref selectedDayCourse, value); }

        public WeekCourse SelectedWeek => WholeData.WeekCourses[WholeData.SelectedWeekId];

        public List<DayCourse> AllDayCourses => GetAllDayCourses();

        public List<WeekCourse> WeekCourses => WholeData.WeekCourses.ToList();

        public List<Course> CourseSpecies => WholeData.CourseSpecies.ToList();

        public DayCourse TodayDayCourse => GetTodayDayCourse();

        public bool WeatherForecastEnabled
        {
            get => WholeData.WeatherForecastEnabled;
            set
            {
                IsUnsaved = true;
                WholeData.WeatherForecastEnabled = value;
                RaisePropertyChanged(() => WeatherForecastEnabled);
            }
        }

        public string WeatherForecastLocation
        {
            get => WholeData.WeatherForecastLocation;
            set
            {
                IsUnsaved = true;
                WholeData.WeatherForecastLocation = value;
                RaisePropertyChanged(() => WeatherForecastLocation);
            }
        }

        public List<string> ColorNames
        {
            get
            {
                var defaultName = Enum.GetNames(typeof(ColorSample));

                var colorSamples = WholeData.ColorConfig?.ColorSamples;
                if (colorSamples == null)
                    return defaultName.ToList();
                var customed = from x in colorSamples select x.Name;
                return customed.Concat(defaultName).ToList();
            }
        }

        public string SelectedColor => (WholeData.ColorConfig?.SelectedColor) ?? DefaultColorName;

        //TODO: method addColor & deleteColor & changeSelectedColor
        #endregion

        #region Private Properties

        /// <summary>
        /// 当前选中的日课程(可能为循环课表中的一天)是周几 0-based
        /// </summary>
        private int SelectedDayOfWeek
        {
            get
            {
                string dayName = StringFormat.DayNameToDayNamePure(SelectedDayCourse.DayName);
                return StringFormat.DayNameToDayId(dayName);
            }
        }

        #endregion

        #region Const

        public const string FileName = "data.json";

        public const string DefaultColorName = "Blue";
        #endregion

        #region Constructor

        public JsonDataProvider(ColorChange colorChange)
        {
            dataPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory ?? "", FileName);

            this.colorChange = colorChange;

            LoadFromFile();

            //TODO: 似乎刚初始化，不能RaisePropertyChange
            //WeatherForecastLocation = "哈尔滨";
            //RaisePropertyChanged(() => WeatherForecastLocation);
        }

        /// <summary>
        /// 从文件加载
        /// </summary>
        private void LoadFromFile()
        {
            //读取数据
            try
            {
                if (File.Exists(dataPath))
                {
                    string json = File.ReadAllText(dataPath);
                    WholeData = JsonConvert.DeserializeObject<WholeData>(json);
                    InitWholeData();

                }
                else
                {
                    DefaultWholeData();
                    InitWholeData();
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.Message);
                throw;
            }
        }

        /// <summary>
        /// 初始化<see cref="WholeData"/>
        /// </summary>
        private void InitWholeData()
        {
            WholeData ??= new WholeData();
            WholeData.DataDescription ??= new DataDescription();
            WholeData.CourseSpecies ??= new List<Course>();
            WholeData.WeekCourses ??= new List<WeekCourse>();
            WholeData.ColorConfig ??=  new ColorConfig();
            WholeData.ColorConfig.ColorSamples ??=  new List<ColorSampleConfig>();
            
        }

        /// <summary>
        /// <see cref="WholeData"/>的默认配置
        /// </summary>
        private void DefaultWholeData()
        {
            AddWeek("默认周表");
            AddCourseSpecies("全名", "简");

        }
        #endregion

        #region Public Methods

        public void ChangeSelectedColor(string name)
        {
            ColorSampleConfig config = WholeData.ColorConfig?.ColorSamples?.SingleOrDefault((x) => x.Name == name);
            if (config != default(ColorSampleConfig))
            {
                colorChange.FromCustomedSample(config);
            }
            else if(Enum.TryParse(name, out ColorSample sample))
            {
                colorChange.FromDefaultSample(sample);
            }
            else
            {
                return;
            }

            WholeData.ColorConfig.SelectedColor = name;
            RaisePropertyChanged(nameof(SelectedColor));
        }

        public void DeleteColor(string name)
        {
            ColorSampleConfig config = WholeData.ColorConfig.ColorSamples.First((x) => x.Name == name);
            WholeData.ColorConfig.ColorSamples.Remove(config);

            if(SelectedColor == name)
            {
                ChangeSelectedColor(DefaultColorName);
            }
            RaisePropertyChanged(nameof(ColorNames));
        }

        public void AddColor(ColorSampleConfig config)
        {
            WholeData.ColorConfig.ColorSamples.Add(config);

            RaisePropertyChanged(nameof(ColorNames));
        }

        public void ChangeSelectedWeek(WeekCourse selectedWeek)
        {
            int selectedWeekId = WholeData.WeekCourses.IndexOf(selectedWeek);

            WholeData.SelectedWeekId = selectedWeekId;

            RaisePropertyChanged(() => AllDayCourses);
            RaisePropertyChanged(() => TodayDayCourse);

            IsUnsaved = true;
        }

        public void AddCirculatingDay()
        {

            // dayName 为 纯Monday之类的
            string dayName = StringFormat.DayNameToDayNamePure(SelectedDayCourse.DayName);

            SelectedWeek.CirculatingCourses ??= new List<CirculatingDayCourse>();

            //寻找该日的循环表
            CirculatingDayCourse target = SelectedWeek.CirculatingCourses
                .SingleOrDefault(t => t.DayName == dayName);

            if (target == default(CirculatingDayCourse))
            {
                //生成一个循环表
                // 计算 本日或下一个到达选中的周几 日期
                int a = (int)DateTime.Today.DayOfWeek;
                int b = StringFormat.DayNameToDayId(dayName);
                int c = (b + 7 - a) % 7;
                DateTime circulatingDate = DateTime.Today.AddDays(c);

                CirculatingDayCourse newCirculating = new CirculatingDayCourse
                {
                    CirculatingId = 0,
                    DayCourses = new List<DayCourse>(),
                    CirculatingDate = circulatingDate,
                    DayName = dayName
                };

                SelectedWeek.CirculatingCourses.Add(newCirculating);
                target = newCirculating;
            }

            DayCourse add = new DayCourse
            {
                DayName = dayName + ":" + (char)(65 + target.DayCourses.Count),
                Courses = new List<Course>(),
            };

            //修改Model中的数据
            target.DayCourses.Add(add);

            //传递给ViewModel更新的信息
            //此时 如果由普通课表转为循环课表 会失去当前的选中
            RaisePropertyChanged(() => AllDayCourses);

            //设置选中为新加的那一项
            SelectedDayCourse = add;

            if (SelectedDayOfWeek == (int)DateTime.Today.DayOfWeek)
            {
                RaisePropertyChanged(() => TodayDayCourse);
            }

            IsUnsaved = true;

        }

        public void DeleteCirculatingDay()
        {
            string dayNamePure = StringFormat.DayNameToDayNamePure(SelectedDayCourse.DayName);
            CirculatingDayCourse circulating = SelectedWeek.CirculatingCourses
                .Single(x => x.DayName == dayNamePure);

            int begin = circulating.DayCourses.FindIndex(x => x.DayName == SelectedDayCourse.DayName);
            circulating.DayCourses.RemoveAt(begin);
            for (int i = begin; i< circulating.DayCourses.Count; i++)
            {
                var name = circulating.DayCourses[i].DayName;
                circulating.DayCourses[i].DayName = StringFormat.DayNameToDayNamePure(name)
                    + ":" + (char)(65 + StringFormat.DayNameToIndex(name) - 1);
            }

            RaisePropertyChanged(() => AllDayCourses);
            DayCourse selectDay = SelectedWeek.DayCourses.Single(x => x.DayName == dayNamePure);
            SelectedDayCourse = selectDay;

            if (SelectedDayOfWeek == (int)DateTime.Today.DayOfWeek)
            {
                RaisePropertyChanged(() => TodayDayCourse);
            }

            IsUnsaved = true;

        }

        public bool IsCirculatingDay()
        {
            if (SelectedDayCourse == null)
                return false;

            //Debug.Print($"IsCirculatingDay selectedDay = {selectedDay.DayName} value = {selectedDay.DayName.Contains(":")}");
            return SelectedDayCourse.DayName.Contains(":");
        }

        public void Save()
        {
            //预处理
            WholeData.DataDescription.UpdateDate = DateTime.Now;

            //已在AddWeekCourse时初始化好一周的课表
            /*foreach (WeekCourse weekCourse in WholeData.WeekCourses)
            {
                foreach (DayOfWeek dayOfWeek in Enum.GetValues(typeof(DayOfWeek)))
                {
                    //如果找不到 第dayOfWeek天的 数据，那么添加一个空的
                    if (!(from dayCourse in weekCourse.DayCourses
                        where dayCourse.DayName == dayOfWeek.ToString()
                        select dayCourse).Any())
                    {
                        weekCourse.DayCourses.Add(new DayCourse
                        {
                            DayName = dayOfWeek.ToString(),
                            Courses = new ObservableCollection<Course>()
                        });
                    }
                }
            }*/
            //保存数据
            try
            {
                string json = JsonConvert.SerializeObject(WholeData, Formatting.Indented);
                System.IO.File.WriteAllText(dataPath, json);
            }
            catch (Exception e)
            {

                Debug.Print(e.Message);
                throw;
            }

            IsUnsaved = false;
        }

        public void ReloadFromFile()
        {
            LoadFromFile();

            // Raise All PropertyChanged
            RaisePropertyChanged(() => SelectedWeek);
            RaisePropertyChanged(() => CourseSpecies);
            RaisePropertyChanged(() => AllDayCourses);
            RaisePropertyChanged(() => TodayDayCourse);
            RaisePropertyChanged(() => WeekCourses);

            IsUnsaved = false;
        }

        public void AddWeek(string addName)
        {
            WeekCourse weekCourse = new WeekCourse
            {
                WeekName = addName,
                DayCourses = new List<DayCourse>(),
                CirculatingCourses = new List<CirculatingDayCourse>(),
            };

            foreach (DayOfWeek dayOfWeek in Enum.GetValues(typeof(DayOfWeek)))
            {
                weekCourse.DayCourses.Add(new DayCourse
                {
                    DayName = dayOfWeek.ToString(),
                    Courses = new List<Course>(),
                });
            }

            WholeData.WeekCourses.Add(weekCourse);

            WholeData.SelectedWeekId = WholeData.WeekCourses.Count - 1;

            RaisePropertyChanged(() => WeekCourses);

            IsUnsaved = true;

        }

        public void DeleteWeek(WeekCourse weekCourse)
        {

            int deleteWeekId = -1;

            for (int i = 0; i < WholeData.WeekCourses.Count; i++)
            {
                if (WholeData.WeekCourses[i] == weekCourse)
                {
                    if (WholeData.SelectedWeekId >= i)
                    {
                        WholeData.SelectedWeekId--;
                    }
                    WholeData.WeekCourses.RemoveAt(i);

                    deleteWeekId = i;
                    break;
                }
            }

            RaisePropertyChanged(() => WeekCourses);

            if (deleteWeekId >= WholeData.SelectedWeekId)
            {
                RaisePropertyChanged(() => SelectedWeek);
            }
            if (deleteWeekId == WholeData.SelectedWeekId)
            {
                if (SelectedDayOfWeek == (int)DateTime.Today.DayOfWeek)
                {
                    RaisePropertyChanged(() => TodayDayCourse);
                }
                RaisePropertyChanged(() => AllDayCourses);
            }

            IsUnsaved = true;

        }

        public void DeleteCourseSpecies(string courseFullName)
        {
            Course course = WholeData.CourseSpecies.FirstOrDefault(x => x.FullName == courseFullName);

            WholeData.CourseSpecies.Remove(course);

            RaisePropertyChanged(() => CourseSpecies);

            IsUnsaved = true;

        }

        public void AddCourseSpecies(string fullName, string showName)
        {
            WholeData.CourseSpecies.Add(new Course { FullName = fullName, ShowName = showName });

            RaisePropertyChanged(() => CourseSpecies);

            IsUnsaved = true;

        }

        public void DeleteCourse(Course selectedCourse)
        {
            SelectedDayCourse.Courses.Remove(selectedCourse);

            RaisePropertyChanged(() => AllDayCourses);
            if (SelectedDayOfWeek == (int)DateTime.Today.DayOfWeek)
            {
                RaisePropertyChanged(() => TodayDayCourse);
            }

            IsUnsaved = true;

        }

        public void AddCourse(string fullName)
        {
            Course course = (from c in WholeData.CourseSpecies
                             where c.FullName == fullName
                             select c).FirstOrDefault();

            SelectedDayCourse.Courses.Add(course);

            RaisePropertyChanged(() => AllDayCourses);
            if (SelectedDayOfWeek == (int)DateTime.Today.DayOfWeek)
            {
                RaisePropertyChanged(() => TodayDayCourse);
            }

            IsUnsaved = true;

        }


        #endregion

        #region Private Methods

        private List<DayCourse> GetAllDayCourses()
        {
            List<DayCourse> result = new List<DayCourse>(SelectedWeek.DayCourses);

            if (SelectedWeek.CirculatingCourses == null) return result;

            foreach (CirculatingDayCourse circulate in SelectedWeek.CirculatingCourses)
            {
                if (circulate.DayCourses.Count == 0)
                    continue;
                int i = 0;
                for (; i < result.Count;)
                {
                    if (result[i].DayName != circulate.DayName)
                    {
                        i++;
                        continue;
                    }
                    result.RemoveAt(i);
                    foreach (var day in circulate.DayCourses)
                    {
                        result.Insert(i, day);
                        i++;
                    }
                    break;
                }

            }
            return result;
        }

        private DayCourse GetTodayDayCourse()
        {
            DayCourse result = null;

            //优先用Circulating的课程。根据规则，如果当天CirculatingCourses不为空，则当天普通Course为空
            if (SelectedWeek.CirculatingCourses != null
                && SelectedWeek.CirculatingCourses.Count != 0)
            {
                foreach (CirculatingDayCourse cir in SelectedWeek.CirculatingCourses)
                {
                    if (cir.DayCourses.Count == 0)
                        continue;
                    if (cir.DayName == DateTime.Today.DayOfWeek.ToString())
                    {
                        int dayGap = (int)(DateTime.Today - cir.CirculatingDate).TotalDays;
                        int idGap = dayGap / 7;
                        cir.CirculatingId = (cir.CirculatingId + idGap) % cir.DayCourses.Count;
                        cir.CirculatingDate = DateTime.Today;
                        result = cir.DayCourses[cir.CirculatingId];
                    }
                }
            }

            result ??= SelectedWeek?.DayCourses.First(x => x.DayName == DateTime.Today.DayOfWeek.ToString());

            result.Courses = result.Courses.ToList();

            result = new DayCourse()
            {
                DayName = result.DayName,
                Courses = result.Courses,
            };
            return result;
        }

        #endregion
    }
}
