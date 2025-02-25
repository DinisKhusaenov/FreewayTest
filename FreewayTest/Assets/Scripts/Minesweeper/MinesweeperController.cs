using System;
using Configs;
using Factory;
using Input;
using UnityEngine;

namespace Minesweeper
{
    public class MinesweeperController : IDisposable
    {
        private CellFactory _cellFactory;
        private GridFactory _gridFactory;
        private GridFiller _gridFiller;
        private Cell[,] _cells;
        private bool _isFirstClick = true;

        private MinesweeperConfig _config;
        private IInputService _inputService;

        public MinesweeperController(Cell prefab, Transform cellsParent, MinesweeperConfig config, IInputService inputService)
        {
            _config = config;
            _inputService = inputService;
            _inputService.OnRestarted += Reset;
            
            _cellFactory = new CellFactory(prefab);
            _gridFactory = new GridFactory(_cellFactory, _config);
            _gridFiller = new GridFiller(_config);
            _cells = _gridFactory.Get(cellsParent);
            
            for (int y = 0; y < _config.YGridSize; y++)
            {
                for (int x = 0; x < _config.XGridSize; x++)
                {
                    var x1 = x;
                    var y1 = y;
                    _cells[x, y].OnLeftCLicked += (value) => OnCellLeftClicked(value, x1, y1);
                }
            }
        }
        
        public void Dispose()
        {
            _inputService.OnRestarted -= Reset;
        }

        private void OnCellLeftClicked(Cell cell, int x, int y)
        {
            cell.Open();

            if (_isFirstClick)
            {
                _gridFiller.Fill(_cells, x, y);
                _isFirstClick = false;
            }

            if (cell.IsBomb)
            {
                Debug.Log("GameOver");
            }
            else if (cell.IsEmpty)
            {
                _gridFiller.OpenEmptyCells(x, y);
            }
        }

        private void Reset()
        {
            for (int y = 0; y < _config.YGridSize; y++)
            {
                for (int x = 0; x < _config.XGridSize; x++)
                {
                    _cells[x, y].Close();
                }
            }

            _isFirstClick = false;
        }
    }
}