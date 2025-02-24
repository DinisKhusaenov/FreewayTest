using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Minesweeper
{
    public class Cell : MonoBehaviour , IPointerClickHandler
    {
        public event Action<Cell> OnLeftCLicked;
        public event Action<Cell> OnRightCLicked;
        
        [SerializeField] private Image _block;
        [SerializeField] private Image _bomb;
        [SerializeField] private Image _flag;
        [SerializeField] private TMP_Text _number;
        [field: SerializeField, Range(1, 500)] public int CellSize { get; private set; }

        private bool _isFlagActive;
        private bool _isClickable = true;

        public bool IsBomb { get; private set; }
        public bool IsEmpty { get; private set; }
        public int Number { get; private set; } = -1;

        public void Initialize(bool isBomb, int number = -1)
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
            _isFlagActive = false;
            _isClickable = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_isClickable) return;

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightCLicked?.Invoke(this);
            }
            else if (eventData.button == PointerEventData.InputButton.Left && !_isFlagActive)
            {
                Block();
                OnLeftCLicked?.Invoke(this);
            }
        }

        private void Block()
        {
            _isFlagActive = !_isFlagActive;
            _isClickable = !_isFlagActive;

            _flag.gameObject.SetActive(_isFlagActive);
        }
    }
}
