namespace KeyboardMouseHooker
{
    class Program
    {
        static void Main(string[] args)
        {
            HookerKeyboard hookerKeyboard = new HookerKeyboard();
            HookerMouse hookerMouse = new HookerMouse();
            hookerKeyboard.SetHook();
            hookerMouse.SetHook();
            hookerKeyboard.Hooked += HookerKeyboard_Hooked;
            hookerMouse.Hooked += HookerMouse_Hooked;
            hookerMouse.Unhook();
            hookerKeyboard.Unhook();
        }

        private static bool HookerMouse_Hooked(HookEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private static bool HookerKeyboard_Hooked(HookEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
