using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace DxfAndPDFViewer
{
    /// <summary>
    /// Interaction logic for LoadLocalFile.xaml
    /// </summary>
    public partial class LoadLocalFile : Window
    {
        public LoadLocalFile()
        {
            InitializeComponent();
        }
        private void LocalLoadWin_Loaded(object sender, RoutedEventArgs e)
        {
            //var arrDot = new int[15];
            //var dot1 = ". ";
            //var dot2 = ". ";

            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = new TimeSpan(0, 0, 0, 1, 0);

            //for (int i = 0; i < arrDot.Count(); i++)
            //{
            //    timer.Start();
            //    dot2 = dot2 + dot1;
            //    LabelGetFile.Content = dot2;


            //    timer.Stop();
            //}
        }

    }
}
