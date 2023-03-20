using Microsoft.VisualBasic;
using System;
using System.Collections;
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
using System.Windows.Threading;

namespace SystemProgramming_111
{
    /// <summary>
    /// Interaction logic for ThreadingWindow.xaml
    /// </summary>
    public partial class ThreadingWindow : Window
    {
        Random random = new Random();
        public ThreadingWindow()
        {
            InitializeComponent();
        }

        private void ButtonStart1_Click(object sender, RoutedEventArgs e)
        {
            Start1();
        }

        private void ButtonStop1_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Start1()
        {
            for (int i = 0; i < 10; i++)
            {
                progressBar1.Value = (i + 1) * 10;
                ConsoleBlock.Text += i.ToString() + "\n";
                Thread.Sleep(300);
            }
        }

        private void ButtonStart2_Click(object sender, RoutedEventArgs e)
        {
            // new Thread(Start1).Start(); ошибка изменения из другого потока
        }

        private void ButtonStop2_Click(object sender, RoutedEventArgs e)
        {

        }
        private bool isStoped3;
        private void ButtonStart3_Click(object sender, RoutedEventArgs e)
        {
            isStoped3 = false;
            new Thread(Start3).Start();
        }

        private void ButtonStop3_Click(object sender, RoutedEventArgs e)
        {
            isStoped3 = true;
        }
        private void Start3()
        {
            for (int i = 0; i < 10 && !isStoped3; i++)
            {
                this.Dispatcher.Invoke(() =>
                {
                    progressBar3.Value = (i + 1) * 10;
                    ConsoleBlock.Text += i.ToString() + "\n";

                });
                Thread.Sleep(300);
            }

        }

        private bool isStoped4;
        private int savedProgress4 = 0;
        private void ButtonStart4_Click(object sender, RoutedEventArgs e)
        {
            isStoped4 = false;
            ButtonStart4.IsEnabled = false;
            
            try
            {
                var t = new Thread(Start4);
                if(t is null)
                {
                    ButtonStart4.IsEnabled = true;
                    return;
                }
                t.Start(savedProgress4);

            } catch (Exception ex){
                MessageBox.Show(ex.Message);
                ButtonStart4.IsEnabled = true;
            }
            
        }

        private void ButtonStop4_Click(object sender, RoutedEventArgs e)
        {
            isStoped4 = true;
            ButtonStart4.IsEnabled = true;
        }
        private void Start4(object? startIndex)
        {

            try
            {
                if (startIndex is int startFrom)
                {
                    for (int i = startFrom; i < 10; i++)
                    {
                        if (isStoped4) return;
                        savedProgress4 = i;
                        this.Dispatcher.Invoke(() =>
                        {
                            progressBar4.Value = (i + 1) * 10;
                            ConsoleBlock.Text += i.ToString() + "\n";

                        });
                        Thread.Sleep(300);
                    }
                    savedProgress4 = 0;
                }
            }
            finally
            {
                this.Dispatcher.Invoke(() =>
                {
                    ButtonStart4.IsEnabled = true;
                });
            }
            /*
 for (int i = start; i < 10; i++)
 {
     if (isStoped4) return;
     this.Dispatcher.Invoke(() =>
     {
         savedProgress4 = i;
         progressBar4.Value = (i + 1) * 10;
         ConsoleBlock.Text += i.ToString() + "\n";

     });
     Thread.Sleep(300);

 }

 savedProgress4 = 0;
 */
        }



    }
}
