using System;
using System.Runtime.InteropServices;

namespace FreeIDE.Common.WinShell
{
    public class Kernel32
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr HeapCreate(uint flOptions, UIntPtr dwInitialSize, UIntPtr dwMaximumSize);

        [DllImport("kernel32.dll")]
        static extern bool HeapDestroy(IntPtr hHeap);

        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory")]
        public static extern void RtlMoveMemory(int des, int src, int count);

        [DllImport("kernel32.dll", EntryPoint = "OpenProcess")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        public extern static int GetProcAddress(int hwnd, string procedureName);

        [DllImport("kernel32.dll", EntryPoint = "GetModuleHandle")]
        public static extern int GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", EntryPoint = "VirtualAllocEx")]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32", EntryPoint = "CreateRemoteThread")]
        public static extern IntPtr CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, uint lpThreadId);

        [DllImport("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        public static extern IntPtr WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, IntPtr lpNumberOfBytesWritten);
    }
}
