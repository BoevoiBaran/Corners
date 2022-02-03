using System;
using Code.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class HudView : MonoBehaviour
    {
        public event Action OnGameRestarted;
        public event Action OnGameSaved;
        
        [SerializeField] private Button restartGameBtn;
        [SerializeField] private Button saveGameBtn;
        [SerializeField] private Text stepText;

        public void Show()
        {
            gameObject.SetSafeActive(true);
        }

        public void Hide()
        {
            gameObject.SetSafeActive(false);
        }
        
        public void SetStepText(string text)
        {
            stepText.text = text;
        }
        
        private void OnEnable()
        {
            restartGameBtn.onClick.AddListener(RestartBtnClickHandler);
            saveGameBtn.onClick.AddListener(SaveBtnClickHandler);
        }

        private void OnDisable()
        {
            restartGameBtn.onClick.RemoveAllListeners();
            saveGameBtn.onClick.RemoveAllListeners();
        }

        private void RestartBtnClickHandler()
        {
            OnGameRestarted?.Invoke();
        }
        
        private void SaveBtnClickHandler()
        {
            OnGameSaved?.Invoke();
        }
    }
}
