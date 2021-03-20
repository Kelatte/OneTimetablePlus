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

            //InitPosition();
        }

        private void InitPosition()
        {
            Rect workArea = System.Windows.SystemParameters.WorkArea;
            this.Left = 0;
            this.Top = (workArea.Height - this.Height) / 2 + workArea.Top;
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
