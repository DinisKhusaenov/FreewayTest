using System;
using Configs;
using Factory;
using Input;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Minesweeper
{
    public class MinesweeperController : IDisposable
    {
        private readonly MinesweeperConfig _config;
        private readonly IInputService _inputService;
        private readonly Camera _camera;
        
        private CompletionPanelFactory _panelFactory;
        private CellFactory _cellFactory;
        private GridFactory _gridFactory;
        private GridFiller _gridFiller;
        
        private Cell[,] _cells;
        private LayerMask _cellLayer;
        private bool _isFirstClick = true;
        private int _openedCells;
        private CompletionPanel _completionPanel;

        public MinesweeperController(Cell prefab, Transform cellsParent, MinesweeperConfig config, 
            IInputService inputService, CompletionPanelFactory panelFactory)
        {
            _config = config;
            _panelFactory = panelFactory;
            _inputService = inputService;
            _cellLayer = prefab.Layer;
            _camera = Camera.main;
            
            _inputService.OnRestarted += Reset;
            _inputService.OnOpenClicked += OnOpenClicked;
            _inputService.OnFlagClicked += OnFlagClicked;
            
            CreateGrid(prefab, cellsParent);
        }
        
        public void Dispose()
        {
            _inputService.OnRestarted -= Reset;
            _inputService.OnOpenClicked -= OnOpenClicked;
            _inputService.OnFlagClicked -= OnFlagClicked;
        }

        private void CreateGrid(Cell prefab, Transform cellsParent)
        {
            _cellFactory = new CellFactory(prefab);
            _gridFactory = new GridFactory(_cellFactory, _config);
            _gridFiller = new GridFiller(_config);
            _cells = _gridFactory.Get(cellsParent);
        }

        private void OnOpenClicked()
        {
            var cell = CheckOnHit();

            if (cell != null)
            {
                OpenCell(cell);
            }
        }
        
        private void OnFlagClicked()
        {
            var cell = CheckOnHit();
            
            if (cell != null)
            {
                BlockCell(cell);
            }
        }

        private Cell CheckOnHit()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return null;
            }
            
            Vector2 mousePosition = _camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0, _cellLayer);

            return hit.collider != null ? hit.collider.GetComponent<Cell>() : null;
        }

        private void OpenCell(Cell cell)
        {
            if (cell.IsOpened || cell.IsFlagActive) return;
            
            cell.Open();

            if (_isFirstClick)
            {
                _gridFiller.Fill(_cells, cell);
                _isFirstClick = false;
            }

            if (cell.IsBomb)
            {
                Lose();
                return;
            }

            if (cell.IsEmpty)
            {
                _gridFiller.OpenEmptyCells((int)cell.GridPosition.x, (int)cell.GridPosition.y , ref _openedCells);
            }
            else
            {
                _openedCells++;
            }

            CheckOnWin();
        }

        private void BlockCell(Cell cell)
        {
            if (!cell.IsOpened)
            {
                cell.Block();
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