﻿using System;
using System.Runtime.InteropServices;

namespace KeyboardMouseHooker
{
    public class KeyboardHookEventArgs : HookEventArgs
    {
        public short VirtualKeyCode { get; set; }
        public int Time { get; set; }
        public int KeyboardMassage { get; set; }
    }

    public class HookerKeyboard : Hooker
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct KeyHookedStruct
        {
            public short VirtualKeyCode;
            public short ScanCode;
            public int Flags;
            public int Time;
            public UIntPtr ExtraInfo;
        }

        private const int WH_KEYBOARD_LL = 13;

        private LowLevelKeyProcDelegate mCallbackKeyboard;

        private IntPtr LowLevelKeyboardHookProc(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                KeyHookedStruct keyStruct = (KeyHookedStruct)Marshal.PtrToStructure(lParam, typeof(KeyHookedStruct));
                int keyboardMessage = (int)wParam;

                KeyboardHookEventArgs keyHookEventArgs = new KeyboardHookEventArgs()
                {
                    VirtualKeyCode = keyStruct.VirtualKeyCode,
                    KeyboardMassage = keyboardMessage,
                    Time = keyStruct.Time
                };

                return OnHooked(keyHookEventArgs) ? CallNextHookEx(mKeyboardHook, nCode, wParam, lParam) : new IntPtr(1);
            }
            else
                return CallNextHookEx(mKeyboardHook, nCode, wParam, lParam);
        }

        private IntPtr mKeyboardHook;

        public override void SetHook()
        {
            //фильтр - колбэк, при перехвате события
            mCallbackKeyboard = LowLevelKeyboardHookProc;
            //дескриптор файла, в котором содержится процедура фильтра, в данном случае 0, чтобы получить дескриптор файла текущего процесса
            IntPtr deskriptorFileProc = GetModuleHandle(IntPtr.Zero);
            //идентификатор потока, с которым должен быть связан хук, в данном случае 0, чтобы связаться со всеми существующими потоками
            int idStream = 0;
            //перехватываем WH_KEYBOARD_LL - события ввода с клавиатуры
            mKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, mCallbackKeyboard, deskriptorFileProc, idStream);
        }

        public override void Unhook()
        {
            //удаляем процедуру фильтра с данным дескриптором из хука
            UnhookWindowsHookEx(mKeyboardHook);
        }
    }
}
