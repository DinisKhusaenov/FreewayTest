using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Minesweeper
{
    public class Cell : MonoBehaviour , IPointerClickHandler
    {
        public event Action<Cell> OnOpenCLicked;
        public event Action<Cell, bool> OnFlagCLicked;
        
        [SerializeField] private Image _block;
        [SerializeField] private Image _bomb;
        [SerializeField] private Image _flag;
        [SerializeField] private TMP_Text _number;

        public RectTransform RectTransform { get; private set; }
        public bool IsBomb { get; private set; }
        public bool IsEmpty { get; private set; }
        public bool IsOpened { get; private set; }
        public bool IsFlagActive { get; private set; }

        public float XSize => RectTransform.sizeDelta.x;
        public float YSize => RectTransform.sizeDelta.y;

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

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

        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsOpened) return;

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Block();
                OnFlagCLicked?.Invoke(this, IsFlagActive);
            }
            else if (eventData.button == PointerEventData.InputButton.Left && !IsFlagActive)
            {
                OnOpenCLicked?.Invoke(this);
            }
        }

        private void Block()
        {
            IsFlagActive = !IsFlagActive;

            _flag.gameObject.SetActive(IsFlagActive);
        }
    }
}
