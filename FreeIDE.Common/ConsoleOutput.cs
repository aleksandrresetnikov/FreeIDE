using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FreeIDE.Common
{
    public class ConsoleOutput
    {
        const string KERNEL32 = "kernel32.dll";

        [DllImport(KERNEL32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport(KERNEL32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FreeConsole();

        public delegate void ConsoleOutputEvent(string value);
        public static event ConsoleOutputEvent WhenWriting;

        [Conditional("DEBUG")]
        public static void Setup()
        {
            //При загрузке настроек включить или выключить
        }

        [Conditional("DEBUG")]
        public static void Init()
        {
            AllocConsole();
        }

        [Conditional("DEBUG")]
        public static void Free()
        {
            FreeConsole();
        }

        [Conditional("DEBUG")]
        public static void Write(string format, params object[] arg)
        {
            Console.Write(format, arg);
            if (WhenWriting != null)
                WhenWriting.Invoke(String.Format(format, arg));
        }

        [Conditional("DEBUG")]
        public static void Write(object value)
        {
            Console.Write(value);
            if (WhenWriting != null)
                WhenWriting.Invoke(value.ToString());
        }

        [Conditional("DEBUG")]
        public static void WriteLine(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
            if (WhenWriting != null)
                WhenWriting.Invoke(String.Format(format, arg));
        }

        [Conditional("DEBUG")]
        public static void WriteLine(object value)
        {
            Console.WriteLine(value);
            if (WhenWriting != null)
                WhenWriting.Invoke(value.ToString());
        }

        [Conditional("DEBUG")]
        public static void Clear()
        {
            Console.Clear();
        }
    }
}
