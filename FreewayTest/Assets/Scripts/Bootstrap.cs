using System;
using Configs;
using Input;
using Minesweeper;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private MinesweeperConfig _config;
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Transform _cellsParent;

    private PlayerInput _playerInput;
    private MinesweeperController _minesweeperController;
    private IInputService _inputService;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Enable();
        _inputService = new KeyboardInput(_playerInput);
        _minesweeperController = new MinesweeperController(_cellPrefab, _cellsParent, _config, _inputService);
    }

    private void OnDestroy()
    {
        if (_inputService is IDisposable disposable)
        {
            disposable.Dispose();
        }
        _playerInput.Disable();
    }
}