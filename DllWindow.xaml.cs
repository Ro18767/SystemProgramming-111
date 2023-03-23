using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
    /// Interaction logic for DllWindow.xaml
    /// </summary>
    public partial class DllWindow : Window
    {


        public DllWindow()
        {
            InitializeComponent();
        }
        [DllImport("User32.dll")]
        public static extern
    int MessageBoxA(
    System.IntPtr hWnd,      // HWND
    System.String lpText,    // LPCSTR
    System.String lpCaption, // LPCSTR
    System.UInt32 uType      // UINT
    );
        private void ButtonMsgA_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxA(
                System.IntPtr.Zero,
                "Message",
                "Title",
                1
                );
        }
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern
int MessageBoxW(
System.IntPtr hWnd,      // HWND
System.String lpText,    // LPCWSTR 
System.String lpCaption, // LPCWSTR
System.UInt32 uType      // UINT
);
        [DllImport("Kernel32.dll", EntryPoint = "Beep")]
        public static extern
bool Sound(uint dwFreq, uint dwDuration);

        private void ButtonMsgW_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxW(
                System.IntPtr.Zero,
                "Message",
                "Title",
                1
                );
        }

        private void ButtonMsgCustom1_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxW(
    System.IntPtr.Zero,
    "Повторить попытку соеденения?",
    "Соеденение не установлено",
    0x35
    );
        }

        private void ButtonMsgCustom2_Click(object sender, RoutedEventArgs e)
        {
            ErrorAlert("Ошибка!");
        }
        private bool? Confirm(String message)
        {
            var res = MessageBoxW(
    System.IntPtr.Zero,
    message,
    null!,
    0x46
    );
            return res switch
            {
                11 => true,
                10 => false,
                _ => null,
            };
        }

        private bool Info(String message)
        {
            var res = MessageBoxW(
    System.IntPtr.Zero,
    message,
    null!,
    0x44
    );
            return res switch
            {
                11 => true,
                _ => false,
            };
        }
        private void ButtonMsgCustom3_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxW(
                System.IntPtr.Zero,
                "Я слежу за тобой.",
                null!,
                0x10
                );
        }
        private void ButtonMsgCustom4_Click(object sender, RoutedEventArgs e)
        {
            var res = Confirm("Согласны?");
        }
        private void ErrorAlert(String message)
        {
            MessageBoxW(
    System.IntPtr.Zero,
    message,
    "",
    0x10
    );
        }
        private bool Ask(String message)
        {
            var res = MessageBoxW(
    System.IntPtr.Zero,
    message,
    "",
    0x44
    );
            return res switch
            {
                6 => true,
                _ => false,
            };
        }
        private void Warn(String message)
        {
            MessageBoxW(
    System.IntPtr.Zero,
    message,
    "",
    0x30
    );
        }
        private void ButtonMsgCustom5_Click(object sender, RoutedEventArgs e)
        {
            var res = Ask("Согласны?");
            if (res)
            {
                Warn("Действие подтверждено");
            } else
            {
                ErrorAlert("Действие отменено");
            }
        }

        private void ButtonBeep1_Click(object sender, RoutedEventArgs e)
        {
            var t = new Thread(() => Sound(256, 1000));
            if (t == null) return;
            t.Start();
        }

        private void ButtonBeep2_Click(object sender, RoutedEventArgs e)
        {
            var t = new Thread(() => Sound(288, 1000));
            if (t == null) return;
            t.Start();
        }

        private void ButtonBeep3_Click(object sender, RoutedEventArgs e)
        {
            var t = new Thread(() => Sound(323, 1000));
            if (t == null) return;
            t.Start();
        }
    }
}
