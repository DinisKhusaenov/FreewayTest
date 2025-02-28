using System;

namespace Input
{
    public class KeyboardInput : IInputService, IDisposable
    {
        public event Action OnRestarted;
        public event Action OnOpenClicked;
        public event Action OnFlagClicked;

        private readonly PlayerInput _input;

        public KeyboardInput(PlayerInput input)
        {
            _input = input;

            _input.PlayerActions.Restart.started += _ => OnRestarted?.Invoke();
            _input.PlayerActions.Open.started += _ => OnOpenClicked?.Invoke();
            _input.PlayerActions.Flag.started += _ => OnFlagClicked?.Invoke();
        }
        
        public void Dispose()
        {
            _input.PlayerActions.Restart.started -= _ => OnRestarted?.Invoke();
            _input.PlayerActions.Open.started -= _ => OnOpenClicked?.Invoke();
            _input.PlayerActions.Flag.started -= _ => OnFlagClicked?.Invoke();
        }
    }
}