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
    /// Interaction logic for MultiWindow.xaml
    /// </summary>
    public partial class MultiWindow : Window
    {

        Random random = new Random();

        private readonly Random Random = new Random();
        public MultiWindow()
        {
            InitializeComponent();
        }

        private double sum;

        private void ButtonStart1_Click(object sender, RoutedEventArgs e)
        {
            sum = 100;
            this.progressBar1.Value = 0;
            for (int i = 0; i < 12; i++)
            {
                new Thread(plusPercent).Start();
            }
        }

        private void ButtonStop1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void plusPercent()
        {
            double val = sum;
            double percent = 10;
            Thread.Sleep(Random.Next(250, 350));
            val *= 1 + percent / 100;

            sum = val;
            Dispatcher.Invoke(() => { 
                ConsoleBlock.Text += sum + "\n";
                this.progressBar1.Value += 1;
            });
        }

        private double sum2;
        private CancellationTokenSource cts;

        private readonly object locker2 = new();


        private void ButtonStart2_Click(object sender, RoutedEventArgs e)
        {
            sum2 = 100;
            this.progressBar2.Value = 0;
            cts = new();
            for (int i = 0; i < 12; i++)
            {
                new Thread(plusPercent2).Start(cts.Token);
            }
        }

        private void ButtonStop2_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
        }

        private void plusPercent2(object? token)
        {
            if (token is not CancellationToken) return;
            CancellationToken cancellationToken = (CancellationToken) token;
            double val;
            lock (locker2)
            {
                if (cancellationToken.IsCancellationRequested) return;
                val = sum2;
                double percent = 10;
                Thread.Sleep(Random.Next(250, 350));
                val *= 1 + percent / 100;
                sum2 = val;
            }

            Dispatcher.Invoke(() => {
                ConsoleBlock.Text += sum2 + "\n";
                this.progressBar2.Value += 1;
            });
        }


        private double sum3;
        private readonly object locker3 = new();


        private void ButtonStart3_Click(object sender, RoutedEventArgs e)
        {
            sum3 = 100;
            progressBar3.Value = 0;
            for (int i = 0; i < 12; i++)
            {
                new Thread(plusPercent3).Start(i + 1);
            }
        }

        private void ButtonStop3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void plusPercent3(object? param)
        {
            if (param is not int) return;
            int month = (int)param;
            double val;
            double percent = 10 + month;
            double factor = 1 + percent / 100;
            Thread.Sleep(Random.Next(250, 350));

            lock (locker3)
            {
                val = sum3;
                val *= factor;
                sum3 = val;
            }

            Dispatcher.Invoke(() => {
                ConsoleBlock.Text += month + " " + percent + " " + val + "\n";
                progressBar3.Value += 1;
            });
        }


        private double sum4;
        private readonly object locker4 = new();


        private void ButtonStart4_Click(object sender, RoutedEventArgs e)
        {
            sum4 = 100;
            progressBar4.Value = 0;
            for (int i = 0; i < 12; i++)
            {
                new Thread(plusPercent4).Start(i + 1);
            }
        }

        private void ButtonStop4_Click(object sender, RoutedEventArgs e)
        {

        }

        private void plusPercent4(object? param)
        {
            if (param is not int) return;
            int month = (int)param;
            double val;
            double percent = 10 + month;
            double factor = 1 + percent / 100;
            Thread.Sleep(Random.Next(250, 350));

            lock (locker3)
            {
                val = sum4;
                val *= factor;
                sum4 = val;
            }

            Dispatcher.Invoke(() => {
                ConsoleBlock.Text += month + " " + percent + " " + val + "\n";
                progressBar4.Value += 1;
            });
        }


        #region Thread Pool
        CancellationTokenSource cts5;
        private void ButtonStart5_Click(object sender, RoutedEventArgs e)
        {
            cts5 = new CancellationTokenSource();
            for (int i = 0; i < 25; i++)
            {
                ThreadPool                     // Пул потоков (new не надо)
                    .QueueUserWorkItem(        // добавление новой задачи
                    plusPercent5,              // (она сразу ставится на исполнение - 
                    new ThreadData3            //  отдельный Start() не нужен)
                    {                          // Дополнительный аргумент также
                        Month = i,             // может быть только один и 
                        Token = cts5.Token     // передается в поток вторым параметром
                    });                        // при добавлении
            }                                  // 
        }
        private void ButtonStop5_Click(object sender, RoutedEventArgs e)
        {
            cts5?.Cancel();
        }
        private double sum5;
        private readonly object locker5 = new();     // объект для синхронизации
        private void plusPercent5(object? data)
        {
            var threadData = data as ThreadData3;
            if (threadData is null) return;
            try
            {
               
                double val;
                for (int i = 0; i < 3; i++)
                {
                    Thread.Sleep(random.Next(250, 350));
                    // место для возможной отмены потока
                    threadData.Token.ThrowIfCancellationRequested();
                }
                double percent = 10 + threadData.Month;
                double factor = 1 + percent / 100;
                lock (locker5)
                {                                      // внутри блока
                    val = sum5;                        // остается часть рассчетов
                    val *= factor;                     // которую нельзя более
                    sum5 = val;                        // разделять
                }
                Dispatcher.Invoke(() =>
                {
                    ConsoleBlock.Text += threadData.Month + " " + percent + " " + val + "\n";
                    progressBar5.Value += 100.0 / 25;
                });
            } catch (OperationCanceledException)
            {
                Dispatcher.Invoke(() =>
                {
                    ConsoleBlock.Text += threadData.Month + " Canceled" +  "\n";
                    progressBar5.Value += 100.0 / 25;
                });
            }
        }
        #endregion
    }
    class ThreadData3   // комплексный тип для передачи нескольких данных в поток
    {
        public int Month { get; set; }
        public CancellationToken Token { get; set; }
    }
}
