using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneTimetablePlus.Models
{
    public class Course
    {
        /// <summary>
        /// 课程全名 例如：语文
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 课程显示名称 例如：语
        /// </summary>
        public string ShowName { get; set; }
    }
}
