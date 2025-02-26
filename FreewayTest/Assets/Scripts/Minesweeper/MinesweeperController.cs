using System;
using Configs;
using Factory;
using Input;
using UI;
using UnityEngine;

namespace Minesweeper
{
    public class MinesweeperController : IDisposable
    {
        private readonly MinesweeperConfig _config;
        private readonly IInputService _inputService;
        
        private CompletionPanelFactory _panelFactory;
        private CellFactory _cellFactory;
        private GridFactory _gridFactory;
        private GridFiller _gridFiller;
        
        private Cell[,] _cells;
        private bool _isFirstClick = true;
        private int _openedCells;
        private CompletionPanel _completionPanel;

        public MinesweeperController(Cell prefab, Transform cellsParent, MinesweeperConfig config, 
            IInputService inputService, CompletionPanelFactory panelFactory)
        {
            _config = config;
            _inputService = inputService;
            _panelFactory = panelFactory;
            
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
                    _cells[x, y].OnOpenCLicked += (value) => OnCellOpenClicked(value, x1, y1);
                }
            }
        }
        
        public void Dispose()
        {
            _inputService.OnRestarted -= Reset;
        }

        private void OnCellOpenClicked(Cell cell, int x, int y)
        {
            cell.Open();

            if (_isFirstClick)
            {
                _gridFiller.Fill(_cells, x, y);
                _isFirstClick = false;
            }

            if (cell.IsBomb)
            {
                Lose();
                return;
            }

            if (cell.IsEmpty)
            {
                _gridFiller.OpenEmptyCells(x, y, ref _openedCells);
            }
            else
            {
                _openedCells++;
            }

            CheckOnWin();
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

            _openedCells = 0;
            _isFirstClick = true;
            if (_completionPanel != null)
            {
                _completionPanel.Hide();
                _completionPanel.OnRestartClicked -= Reset;
            }
        }

        private void CheckOnWin()
        {
            if (_openedCells >= _config.CellsCount - _config.BombsCount)
            {
                CreateCompletionPanel();
            }
        }

        private void Lose()
        {
            if (_completionPanel != null)
            {
                _completionPanel.Show();
                _completionPanel.OnRestartClicked += Reset;
            }
            else
            {
                CreateCompletionPanel();
            }
        }

        private void CreateCompletionPanel()
        {
            _completionPanel = _panelFactory.Get();
            _completionPanel.Show();
            _completionPanel.OnRestartClicked += Reset;
        }
    }
}