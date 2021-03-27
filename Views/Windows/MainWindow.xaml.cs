using System;
using Hardcodet.Wpf.TaskbarNotification;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Drawing;
using WpfAppBar;

namespace OneTimetablePlus.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Debug.Print("MainWindow Initialized");

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
#if !DEBUG
            AppBarFunctions.SetAppBar(this, ABEdge.Left, null, false);
#endif
        }

        private void Window_Closing(object sender, EventArgs e)
        {
#if !DEBUG
            AppBarFunctions.SetAppBar(this, ABEdge.None);
#endif
        }
    }

}
