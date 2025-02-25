using Configs;
using Minesweeper;
using UnityEngine;

namespace Factory
{
    public class GridFactory
    {
        private CellFactory _cellFactory;
        private MinesweeperConfig _config;

        public GridFactory(CellFactory cellFactory, MinesweeperConfig config)
        {
            _cellFactory = cellFactory;
            _config = config;
        }

        public Cell[,] Get(Transform cellParent)
        {
            Cell[,] cells = new Cell[_config.XGridSize, _config.YGridSize];

            for (int y = 0; y < _config.YGridSize; y++)
            {
                for (int x = 0; x < _config.XGridSize; x++)
                {
                    var cell = _cellFactory.Get(cellParent);

                    float posX = (x - _config.XGridSize / 2f) * (cell.XSize + _config.SpaceBetweenCells);
                    float posY = (-y + _config.YGridSize / 2f) * (cell.YSize + _config.SpaceBetweenCells);

                    cell.RectTransform.anchoredPosition = new Vector2(posX, posY);

                    cells[x, y] = cell;
                }
            }

            return cells;
        } 
    }
}