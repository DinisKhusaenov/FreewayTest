using System;

namespace Input
{
    public class KeyboardInput : IInputService, IDisposable
    {
        public event Action OnRestarted;
        
        private readonly PlayerInput _input;

        public KeyboardInput(PlayerInput input)
        {
            _input = input;

            _input.PlayerActions.Restart.started += _ => OnRestarted?.Invoke();
        }
        
        public void Dispose()
        {
            _input.PlayerActions.Restart.started -= _ => OnRestarted?.Invoke();
        }
    }
}