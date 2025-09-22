using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace ScreenAnnotation
{
    public static class HotkeyHelper
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public static class Modifiers
        {
            public const uint None = 0;
            public const uint Alt = 1;
            public const uint Ctrl = 2;
            public const uint Shift = 4;
            public const uint WinKey = 8;
        }

        public static class Keys
        {
            public const uint A = 0x41;
            // Add other virtual key codes here if needed
        }
    }
}
