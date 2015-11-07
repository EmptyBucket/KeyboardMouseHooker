namespace KeyboardMouseHooker
{
    public interface IHooker
    {
        void SetHook();

        void Unhook();

        event HookEventHandler Hooked;
    }
}