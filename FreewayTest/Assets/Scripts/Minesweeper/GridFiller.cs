using Configs;
using UnityEngine;

namespace Minesweeper
{
    public class GridFiller
    {
        private MinesweeperConfig _config;
        private Cell[,] _cells;

        public GridFiller(MinesweeperConfig config)
        {
            _config = config;
        }

        public void Fill(Cell[,] cells, int x, int y)
        {
            _cells = cells;
            
            FillWithBombs(x, y);
            FillEmpty();
            FillWithNumbers();
        }

        public void OpenEmptyCells(int x, int y)
        {
            _cells[x, y].Open();

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
                        
                        if (!_cells[i, j].IsOpened)
                            OpenEmptyCells(i, j);
                    }
                }
            }
        }
        
        private void FillWithBombs(int x, int y)
        {
            for (int i = 0; i < _config.BombsCount; i++)
            {
                while (true)
                {
                    var randomX = Random.Range(0, _config.XGridSize);
                    var randomY = Random.Range(0, _config.YGridSize);

                    if (!_cells[randomX, randomY].IsBomb && (randomX != x && randomY != y))
                    {
                        _cells[randomX, randomY].Initialize(true);
                        break;
                    }
                }
            }
        }

        private void FillEmpty()
        {
            int emptyCount = Random.Range(0, _config.CellsCount - _config.BombsCount);
            
            for (int i = 0; i < emptyCount; i++)
            {
                while (true)
                {
                    var randomX = Random.Range(0, _config.XGridSize);
                    var randomY = Random.Range(0, _config.YGridSize);

                    if (!_cells[randomX, randomY].IsBomb)
                    {
                        _cells[randomX, randomY].Initialize(false);
                        break;
                    }
                }
            }
        }

        private void FillWithNumbers()
        {
            for (int y = 0; y < _config.YGridSize; y++)
            {
                for (int x = 0; x < _config.XGridSize; x++)
                {
                    if (!_cells[x, y].IsBomb && !_cells[x, y].IsEmpty)
                    {
                        FillCellWithNumber(x, y);
                    }
                }
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
            
            _cells[x, y].Initialize(false, counter);
        }
    }
}