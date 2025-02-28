using System.Collections.Generic;
using System.Linq;
using Configs;
using UnityEngine;

namespace Minesweeper
{
    public class GridFiller
    {
        private MinesweeperConfig _config;
        private Cell[,] _cells;
        private List<Cell> _uninitializedCells = new();

        public GridFiller(MinesweeperConfig config)
        {
            _config = config;
        }

        public void Fill(Cell[,] cells, Cell cell)
        {
            _cells = cells;
            _uninitializedCells = cells.Cast<Cell>().ToList();
            _uninitializedCells.Remove(cell);

            FillWithBombs();
            _uninitializedCells.Add(cell);
            FillEmpty();
            FillWithNumbers();
        }

        public void OpenEmptyCells(int x, int y, ref int openedCells)
        {
            _cells[x, y].Open();
            
            if (!_cells[x, y].IsBomb)
                openedCells++;

            if (!_cells[x, y].IsEmpty)
                return;
            
            for (int i = x - 1; i <= x + 1; i++)
            {
                if (i < 0 || i >= _config.XGridSize)
                {
                    continue;
                }
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (j >= 0 && j < _config.YGridSize)
                    {
                        if (i == x && j == y) continue;
                        
                        if (!_cells[i, j].IsOpened && !_cells[i, j].IsFlagActive)
                            OpenEmptyCells(i, j, ref openedCells);
                    }
                }
            }
        }
        
        private void FillWithBombs()
        {
            for (int i = 0; i < _config.BombsCount; i++)
            {
                var randomCellId = Random.Range(0, _uninitializedCells.Count);
                _uninitializedCells[randomCellId].Set(true);
                _uninitializedCells.RemoveAt(randomCellId);
            }
        }

        private void FillEmpty()
        {
            int emptyCount = Random.Range(0, _config.CellsCount - _config.BombsCount);
            
            for (int i = 0; i < emptyCount; i++)
            {
                var randomCellId = Random.Range(0, _uninitializedCells.Count);
                _uninitializedCells[randomCellId].Set(false);
                _uninitializedCells.RemoveAt(randomCellId);
            }
        }

        private void FillWithNumbers()
        {
            foreach (var cell in _uninitializedCells)
            {
                FillCellWithNumber((int)cell.GridPosition.x, (int)cell.GridPosition.y);
            }
        }

        private void FillCellWithNumber(int x, int y)
        {
            int counter = 0;

            for (int i = x - 1; i <= x + 1; i++)
            {
                if (i < 0 || i >= _config.XGridSize)
                {
                    continue;
                }
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (j >= 0 && j < _config.YGridSize)
                    {
                        if (_cells[i, j].IsBomb)
                        {
                            counter++;
                        }
                    }
                }
            }

            if (counter > 0)
            {
                _cells[x, y].Set(false, counter);
            }
            else
            {
                _cells[x, y].Set(false);
            }
        }
    }
}