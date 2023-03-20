using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SystemProgramming_111
{
    /// <summary>
    /// Interaction logic for SynhroWindow.xaml
    /// </summary>
    public partial class SynhroWindow : Window
    {
        private void ConsoleWrite(object stringLike)
        {

            Dispatcher.Invoke(() =>
            {
                if (stringLike is null)
                {
                    ConsoleBlock.Text += "NULL";
                    return;
                }
                ConsoleBlock.Text += $"{stringLike}";
            });
        }
        public SynhroWindow()
        {
            InitializeComponent();
        }
        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
        }



        #region lock
        private object locker1 = new ();
        private async void ButtonStart1_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                new Thread(Proc1).Start(i);
            }
            

        }
        private void Proc1(object? item)
        {
            lock (locker1)
            {
                string n = item?.ToString() ?? "NULL\n";
                ConsoleWrite($"Proc{n} Start\n");

                Thread.Sleep(1000);

                ConsoleWrite($"Proc{n} Finish\n");
            }
        }
        #endregion lock
        #region monitor

        private object monitor2 = new();
        private async void ButtonStart2_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                new Thread(Proc2).Start(i);
            }


        }
        private void Proc2(object? item)
        {
            try
            {
                Monitor.Enter(monitor2);

                string n = item?.ToString() ?? "NULL\n";
                ConsoleWrite($"Proc{n} Start\n");

                Thread.Sleep(1000);

                ConsoleWrite($"Proc{n} Finish\n");
            }
            finally
            {
                Monitor.Exit(monitor2);
            }
        }
        #endregion monitor


        #region mutex

        private Mutex mutex3 = new();

        private async void ButtonStart3_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                new Thread(Proc3).Start(i);
            }


        }
        private void Proc3(object? item)
        {

            try
            {
                mutex3.WaitOne();
                string n = item?.ToString() ?? "NULL\n";
                ConsoleWrite($"Proc{n} Start\n");

                Thread.Sleep(1000);

                ConsoleWrite($"Proc{n} Finish\n");
            }
            finally
            {
                mutex3.ReleaseMutex();
            }
                
            
        }
        #endregion mutex

        #region event

        private EventWaitHandle gates4 = new AutoResetEvent(false);

        private async void ButtonStart4_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                new Thread(Proc4).Start(i);
            }

            gates4.Set();

        }
        private void Proc4(object? item)
        {

            try
            {
                gates4.WaitOne();
                string n = item?.ToString() ?? "NULL\n";
                ConsoleWrite($"Proc{n} Start\n");

                Thread.Sleep(1000);

                ConsoleWrite($"Proc{n} Finish\n");
            }
            finally
            {
                gates4.Set();
            }
        }
        #endregion event

        #region sema

        private Semaphore sema5 = new Semaphore(3, 3);

        private async void ButtonStart5_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                new Thread(Proc5).Start(i);
            }

        }
        private void Proc5(object? item)
        {

            try
            {
                sema5.WaitOne();
                string n = item?.ToString() ?? "NULL\n";
                ConsoleWrite($"Proc{n} Start\n");

                Thread.Sleep(1000);

                ConsoleWrite($"Proc{n} Finish\n");
            }
            finally
            {
                sema5.Release();
            }
        }
        #endregion sema
        private async Task<String> FuncN(int N)
        {

            await Task.Delay(1000);

            return $"Func{N} return\n";
        }
    }
}
