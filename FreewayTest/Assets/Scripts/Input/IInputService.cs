using System;

namespace Input
{
    public interface IInputService
    {
        event Action OnRestarted;
    }
}