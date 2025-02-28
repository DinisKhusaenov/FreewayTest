using System;

namespace Input
{
    public interface IInputService
    {
        event Action OnRestarted;
        event Action OnOpenClicked;
        event Action OnFlagClicked;
    }
}