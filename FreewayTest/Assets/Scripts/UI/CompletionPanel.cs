using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CompletionPanel : MonoBehaviour
    {
        public event Action OnRestartClicked;

        [SerializeField] private Button restart;

        public void Show()
        {
            gameObject.SetActive(true);
            
            restart.onClick.AddListener(RestartClicked);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
            
            restart.onClick.RemoveListener(RestartClicked);
        }

        private void RestartClicked()
        {
            OnRestartClicked?.Invoke();
        }
    }
}