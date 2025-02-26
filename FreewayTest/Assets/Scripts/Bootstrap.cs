using System;
using Configs;
using Factory;
using Input;
using Minesweeper;
using UI;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private MinesweeperConfig _config;
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private Transform _cellsParent;
    [SerializeField] private CompletionPanel _completionPanel;
    [SerializeField] private Canvas _canvas;

    private PlayerInput _playerInput;
    private MinesweeperController _minesweeperController;
    private IInputService _inputService;
    private CompletionPanelFactory _completionPanelFactory;

    private void Awake()
    {
        InitInput();
        _completionPanelFactory = new CompletionPanelFactory(_completionPanel, _canvas.transform);
        
        _minesweeperController = new MinesweeperController(_cellPrefab, _cellsParent, _config, 
            _inputService, _completionPanelFactory);
    }

    private void InitInput()
    {
        _playerInput = new PlayerInput();
        _playerInput.Enable();
        _inputService = new KeyboardInput(_playerInput);
    }

    private void OnDestroy()
    {
        if (_inputService is IDisposable disposable)
        {
            disposable.Dispose();
        }
        _playerInput.Disable();
        _minesweeperController.Dispose();
    }
}