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
            
            float halfWidth = (_config.XGridSize - 1) / 2f;
            float halfHeight = (_config.YGridSize - 1) / 2f;

            for (int y = 0; y < _config.YGridSize; y++)
            {
                for (int x = 0; x < _config.XGridSize; x++)
                {
                    var cell = _cellFactory.Get(cellParent);
                    cell.Initialize(x, y);

                    float posX = (x - halfWidth) * (cell.XSize + _config.SpaceBetweenCells);
                    float posY = (-y + halfHeight) * (cell.YSize + _config.SpaceBetweenCells);

                    cell.transform.position = new Vector2(posX, posY);

                    cells[x, y] = cell;
                }
            }

            return cells;
        } 
    }
}