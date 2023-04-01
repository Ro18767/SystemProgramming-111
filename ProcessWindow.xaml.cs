using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Xml.Linq;

namespace SystemProgramming_111
{
    /// <summary>
    /// Interaction logic for ProcessWindow.xaml
    /// </summary>
    public partial class ProcessWindow : Window
    {
        public ProcessWindow()
        {
            InitializeComponent();
        }
        private void ConsoleWrite(object stringLike)
        {

            Dispatcher.Invoke(() =>
            {
                //if (stringLike is null)
                //{
                //    ConsoleBlock.Text += "NULL";
                //    return;
                //}
                //ConsoleBlock.Text += $"{stringLike}";
            });
        }

        #region lock
        private object locker1 = new();
        private void ButtonStart1_Click(object sender, RoutedEventArgs e)
        {
            ButtonStart1.IsEnabled = false;
            new Thread(UpdateProc).Start();
            
        }
        private async void UpdateProc()
        {
            await Dispatcher.BeginInvoke(() =>
            {
                ButtonStart1.IsEnabled = false;
            });
            System.Collections.Generic.List<TreeViewItem> list = new();
            foreach (Process process in Process.GetProcesses())
            {
                await Dispatcher.BeginInvoke(() => {
                    TreeViewItem node = node = new TreeViewItem();

                    if (node is null) return;

                    var node1 = list.Find((p) => p.Header.ToString() == process.ProcessName);
                    if (node1 is null)
                    {

                        node1 = new();
                        node1.Header = process.ProcessName;

                        list.Add(node1);
                    }

                    node.Header = $"{process.Id} {process.ProcessName}";
                    node1.Items.Add(node);
                });
                
            }

            foreach (TreeViewItem node in list)
            {
                await Dispatcher.BeginInvoke(() => {
                    treeView.Items.Add(node);
                });
                
            }
            await Dispatcher.BeginInvoke(() =>
            {
                ButtonStart1.IsEnabled = true;
            });
        }
        //new Thread(Proc1).Start(i);
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

        Process p;
        private void StartNotepad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                var p = Process.Start("notepad.exe", openFileDialog.FileName);
            } else
            {
                var p = Process.Start("notepad.exe");
            }
                
        }

        Process b;
        private void StartB_Click(object sender, RoutedEventArgs e)
        {
           
            string myString = bUrlTextBox.Text;
            if (myString == String.Empty)
            {
                var p = Process.Start(@"C:\Program Files\Google\Chrome\Application\chrome.exe", myString);
            } else
            {
                var p = Process.Start(@"C:\Program Files\Google\Chrome\Application\chrome.exe");
            }
        }
    }
}
