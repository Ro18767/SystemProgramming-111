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
        #region DllFunctions

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
            }
            else
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
        [DllImport("Kernel32.dll", EntryPoint = "CreateThread")]
        public static extern
            IntPtr CreateThread(
                    IntPtr lpThreadAttributes,  // указатель на структуру с параметрами безопасности (NULL)
                    uint dwStackSize,         // граничный размер стека - 0 (по умолчанию)
              ThreadMethod lpStartAddress,      // указатель на стартовый адрес (функции)
                    IntPtr lpParameter,         // указатель на объект с параметрами для ф-ции
                    uint dwCreationFlags,     // флаги запуска - 0 (по умолчанию)
                    IntPtr lpThreadId           // возврат id потока (NULL - не возвращать)
            );
        // главный вопрос - как получить адрес метода в .NET и передать его в неуправляемый код
        // 1. Описываем делегат по документации на функцию (CreateThread)
        public delegate void ThreadMethod();
        // 2. Заменяем IntPtr в декларации ф-ции на делегат ThreadMethod
        // 3. Описываем метод с сигнатурой делегата
        public void SayHello()
        {
            Dispatcher.Invoke(() =>
            {
                SayHelloLabel.Content = "tewg";
            });
        }

        private void SayHelloBtn_Click(object sender, RoutedEventArgs e)
        {
            CreateThread(IntPtr.Zero, 0, SayHello, IntPtr.Zero, 0, IntPtr.Zero);

        }
        // 4. При вызове ф-ции CreateThread в параметре lpStartAddress указываем SayHello
        //    (см. )
        #endregion DllFunctions


        #region MM Timer
        // делегат для передачи адреса периодически вызываемого метода
        delegate void TimerMethod(uint uTimerID, uint uMsg, ref uint dwUser,
            uint dw1, uint dw2);

        [DllImport("winmm.dll", EntryPoint = "timeSetEvent")]
        static extern uint TimeSetEvent(
            uint uDelay,
            uint uResolution,
            TimerMethod lpTimeProc,
            ref uint dwUser,
            uint eventType
            );

        [DllImport("winmm.dll", EntryPoint = "timeKillEvent")]
        static extern void TimeKillEvent(uint uTimerID);

        const uint TIME_ONESHOT = 0;  // eventType
        const uint TIME_PERIODIC = 1;

        uint uDelay;
        uint uResolution;
        uint timerId;
        uint dwUser = 0;
        TimerMethod timerMethod = null!;
        GCHandle timerHandle;

        int ticks;
        void TimerTick(uint uTimerID, uint uMsg, ref uint dwUser, uint dw1, uint dw2)
        {
            ticks++;

            var now = DateTime.Now;
            var hours = now.Hour;
            var minutes = now.Minute;
            var seconds = now.Second;
            var milliseconds = now.Millisecond;
            Dispatcher.Invoke(() => { TicksLabel.Content = $"{hours.ToString("00")}:{minutes.ToString("00")}:{seconds.ToString("00")}.{(milliseconds / 10).ToString("00")}"; });
        }

        private void StartTimerBtn_Click(object sender, RoutedEventArgs e)
        {
            uDelay = 10;      // задержка между вызовами 10 ms (10 Hz)
            uResolution = 10;  // допустимое отклонение (погрешность) для uDelay
            ticks = 0;
            timerMethod = new TimerMethod(TimerTick);
            timerHandle = GCHandle.Alloc(timerMethod);
            timerId = TimeSetEvent(uDelay, uResolution, timerMethod, ref dwUser, TIME_PERIODIC);
            if (timerId != 0)
            {
                StopTimerBtn.IsEnabled = true;
                StartTimerBtn.IsEnabled = false;
            }
            else
            {
                timerHandle.Free();
                timerMethod = null!;
            }
        }

        private void StopTimerBtn_Click(object sender, RoutedEventArgs e)
        {
            StopTimer();
        }
        private void StopTimer()
        {
            TimeKillEvent(timerId);
            timerHandle.Free();
            StopTimerBtn.IsEnabled = false;
            StartTimerBtn.IsEnabled = true;
        }
        /* Мультимедийный таймер:
         * Добавить событие закрытия окна, в нем проверить активность таймера - 
         *  если активный, то выключить (и освободить объект)
         * Реализовать "часы-таймер" в форме 00:00:00.00 (чч:мм:сс.дс)
         * дс - две цифры для сотых долей секунды
         * Запустить таймер с интервалом 10 мс, обработать его тики в виде времени
         */

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StopTimer();
        }
        #endregion


    }
}
