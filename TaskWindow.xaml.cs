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
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        Random random = new Random();
        private void ConsoleWrite(object stringLike)
        {

            Dispatcher.Invoke(() =>
            {
                if(stringLike is null)
                {
                    ConsoleBlock.Text += "NULL";
                    return;
                }
                ConsoleBlock.Text += $"{stringLike}";
            });
        }
        public TaskWindow()
        {
            InitializeComponent();
        }
        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {

        }
        private async void ButtonStart1_Click(object sender, RoutedEventArgs e)
        {
            sum = 100;
            ConsoleBlock.Text = "";
            for (int i = 0; i < 12; i++)
            {
                // Task.Run(PlusPercent).Wait();
                await PlusPercent();
            }
        }
    
        private double sum;
        private async Task PlusPercent()
        {
            await Task.Delay(300);
            sum *= 1.1;
            ConsoleBlock.Text += $"{sum}\n";
            // Dispatcher.Invoke(() => ConsoleBlock.Text += $"{sum}\n");
        }
        private void Demo1_Click(object sender, RoutedEventArgs e)
        {
            
            Task task = new Task(ProcN, 1);
            task.Start();
        }

        private void Demo2_Click(object sender, RoutedEventArgs e)
        {

            //task.Start();
            //task.RunSynchronously();
            Task task = new Task(ProcN, 1);
            Task task2 = new Task(ProcN, 2);

            //task.RunSynchronously();
            //task2.RunSynchronously();

            task.RunSynchronously();
            task2.RunSynchronously();
        }

        private void Demo3_Click(object sender, RoutedEventArgs e)
        {

            // Task.Ru(Proc1).ContinueWith(() => { Proc1()}, TaskContinuationOptions.OnlyOnRanToCompletion);

            Task task = new Task(ProcN, 1);
            Task task2 = new Task(ProcN, 2);

            task.Start();
            task.Wait();
            task2.Start();
           
        }

        private void Demo4_Click(object sender, RoutedEventArgs e)
        {


            Task task = new Task(ProcN, 1);
            Task task2 = new Task(ProcN, 2);

            task.Start();
            task.ContinueWith(_ => task2.Start());

        }

        private  void Demo1_2_Click(object sender, RoutedEventArgs e)
        {
            var task1 = FuncN(1);
            
            
            ConsoleWrite(task1.Result); // зависание
        }

        private async void Demo2_2_Click(object sender, RoutedEventArgs e)
        {
            ConsoleWrite("Demo2_2_Click\n");
            ConsoleWrite(await FuncN(2));
        }

        private async void Demo3_2_Click(object sender, RoutedEventArgs e)
        {
            ConsoleWrite("Demo3_2_Click\n");
            ConsoleWrite(await FuncN(1));
            ConsoleWrite("Demo3_2_Click\n");
            ConsoleWrite(await FuncN(2));
        }

        private async void Demo4_2_Click(object sender, RoutedEventArgs e)
        {
            var task1 = FuncN(1);
            var task2 = FuncN(2);
            ConsoleWrite("Demo3_2_Click\n");
            ConsoleWrite("Demo3_2_Click\n");
            ConsoleWrite(await task1);
            ConsoleWrite(await task2);


        }

        private async void Demo5_2_Click(object sender, RoutedEventArgs e)
        {
            System.Collections.Generic.List<Task<String>> list = new();
            for (int i = 1; i <= 10; i++)
            {
                ConsoleWrite("Demo5_2_Click\n");
                list.Add(FuncN(i));

            }
            foreach (var task in list)
            {
                    ConsoleWrite(await task);
            }

        }
        private void ProcN(object? item)
        {
            string n = item?.ToString() ?? "NULL\n";
            ConsoleWrite($"Proc{n} Start\n");

            Thread.Sleep(1000);

            ConsoleWrite($"Proc{n} Finish\n");
        }

        private async Task<String> FuncN(int N)
        {

            await Task.Delay(1000);

            return $"Func{N} return\n";
        }

        #region dz 9.03.2023

        CancellationTokenSource cts5;
        private async void ButtonStart5_Click(object sender, RoutedEventArgs e)
        {
            cts5 = new CancellationTokenSource();
            int i = 0;
            int allMounthCount = 25;
            ConsoleBlock.Text = "";
            progressBar5.Value = 0;
            sum5 = 100;
            try
            {
                for (; i < allMounthCount; i++)
                {
                    await plusPercent5(new TaskData5
                    {
                        Month = i,
                        Token = cts5.Token
                    });
                    cts5.Token.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException)
            {
                Dispatcher.Invoke(() =>
                {
                    ConsoleBlock.Text += i + "/" + allMounthCount + " Canceled" + "\n";
                    progressBar5.Value += 100.0 / 25;
                });
            }
        }
        private void ButtonStop5_Click(object sender, RoutedEventArgs e)
        {
            cts5?.Cancel();
        }
        private double sum5;
        private readonly object locker5 = new();     // объект для синхронизации
        private async Task plusPercent5(object? data)
        {
            var threadData = data as TaskData5;
            if (threadData is null) return;
            try
            {

                double val;

                await Task.Delay(random.Next(250, 350));
                // место для возможной отмены потока
                threadData.Token.ThrowIfCancellationRequested();
                double percent = 10;
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
            }
            catch (OperationCanceledException)
            {
                Dispatcher.Invoke(() =>
                {
                    ConsoleBlock.Text += threadData.Month + " Canceled" + "\n";
                    progressBar5.Value += 100.0 / 25;
                });
            }
        }
        class TaskData5   // комплексный тип для передачи нескольких данных в задачю
        {
            public int Month { get; set; }
            public CancellationToken Token { get; set; }
        }
        #endregion dz 9.03.2023
    }

}
