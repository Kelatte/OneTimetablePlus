using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Win32;

namespace OneTimetablePlus.Helper
{
    static class AutoStart
    {
        private const string AutoStartKeyName = "OneTimetablePlus";
        
        private static readonly string StartupPath = Process.GetCurrentProcess().MainModule?.FileName;

        /// <summary>
        /// 获取有无设置为自启
        /// </summary>
        /// <returns></returns>
        public static bool IfAutoStart()
        {
            RegistryKey rk = Registry.CurrentUser;
            RegistryKey rk2 = rk.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");
            string value = (string)rk2?.GetValue(AutoStartKeyName);
            //把之前的其他地址改到现在的地址
            if (value != null && value != StartupPath)
            {
                ChangeAutoStart(true);
            }
            return value != null;

        }

        /// <summary>
        /// 修改 是否自启
        /// </summary>
        /// <param name="set"></param>
        public static void ChangeAutoStart(bool set)
        {
            RegistryKey rk = Registry.CurrentUser;
            RegistryKey rk2 = rk.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run");

            if (set)
            {
                rk2?.SetValue(AutoStartKeyName, StartupPath);
            }
            else
            {
                rk2?.DeleteValue(AutoStartKeyName);
            }
        }
    }
}
