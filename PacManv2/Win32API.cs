﻿using System;
using System.Runtime.InteropServices;

namespace Daw1.DavidG.PacManv2
{
    static class Win32API
    {
        [DllImport("user32.dll")]
        public static extern ushort GetKeyState(short nVirtKey);

        public const ushort keyDownBit = 0x80;

        public static bool IsKeyPressed(ConsoleKey key)
        {
            return ((GetKeyState((short)key) & keyDownBit) == keyDownBit);
        }
    }
}
