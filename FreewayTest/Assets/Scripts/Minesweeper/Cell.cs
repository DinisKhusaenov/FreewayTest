using TMPro;
using UnityEngine;

namespace Minesweeper
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _block;
        [SerializeField] private SpriteRenderer _bomb;
        [SerializeField] private SpriteRenderer _flag;
        [SerializeField] private TMP_Text _number;
        [SerializeField] private LayerMask _layer;

        public bool IsBomb { get; private set; }
        public bool IsEmpty { get; private set; }
        public bool IsOpened { get; private set; }
        public bool IsFlagActive { get; private set; }
        public Vector2 GridPosition { get; private set; }
        public LayerMask Layer => _layer;

        public float XSize => transform.localScale.x;
        public float YSize => transform.localScale.y;

        public void Initialize(int x, int y)
        {
            GridPosition = new Vector2(x, y);
        }

        public void Set(bool isBomb, int number = -1)
        {
            if (isBomb)
            {
                _bomb.gameObject.SetActive(true);
                IsBomb = true;
            }
            else if (number > -1)
            {
                _number.gameObject.SetActive(true);
                _number.SetText(number.ToString());
            }
            else
            {
                IsEmpty = true;
            }
        }

        public void Open()
        {
            _block.gameObject.SetActive(false);
            _flag.gameObject.SetActive(false);
            IsFlagActive = false;
            IsOpened = true;
        }

        public void Close()
        {
            _block.gameObject.SetActive(true);
            _bomb.gameObject.SetActive(false);
            _number.gameObject.SetActive(false);
            _flag.gameObject.SetActive(false);
            
            IsOpened = false;
            IsBomb = false;
            IsEmpty = false;
            IsFlagActive = false;
        }

        public void Block()
        {
            IsFlagActive = !IsFlagActive;

            _flag.gameObject.SetActive(IsFlagActive);
        }
    }
}
