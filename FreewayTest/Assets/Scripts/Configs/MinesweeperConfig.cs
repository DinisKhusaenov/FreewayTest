using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "MinesweeperConfig", menuName = "Configs/MinesweeperConfig")]
    public class MinesweeperConfig : ScriptableObject
    {
        [field: SerializeField, Range(0, 30)] public int XGridSize { get; private set; }
        [field: SerializeField, Range(0, 30)] public int YGridSize { get; private set; }
        [field: SerializeField, Range(0, 30)] public float SpaceBetweenCells { get; private set; }
        [field: SerializeField, Range(0, 1000)] public int BombsCount { get; private set; }

        public int CellsCount => XGridSize * YGridSize;

        private void OnValidate()
        {
            if (BombsCount > CellsCount)
            {
                BombsCount = CellsCount;
            }
        }
    }
}