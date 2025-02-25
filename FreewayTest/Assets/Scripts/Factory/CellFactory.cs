using Minesweeper;
using UnityEngine;

namespace Factory
{
    public class CellFactory
    {
        private Cell _cellPrefab;

        public CellFactory(Cell cellPrefab)
        {
            _cellPrefab = cellPrefab;
        }

        public Cell Get(Transform parent)
        {
            var cell = Object.Instantiate(_cellPrefab, parent);

            return cell;
        }
    }
}