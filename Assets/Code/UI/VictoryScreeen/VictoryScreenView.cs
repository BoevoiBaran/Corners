using System;
using Code.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class VictoryScreenView : MonoBehaviour
    {
        public event Action OnGameRestarted;
        
        [SerializeField] private Button restartButton;
        [SerializeField] private Text victoryScreenLabel;
        
        public void Show()
        {
            gameObject.SetSafeActive(true);
        }

        public void Hide()
        {
            gameObject.SetSafeActive(false);
        }

        public void SetVictoryScreenText(string text)
        {
            victoryScreenLabel.text = text;
        }
        
        private void OnEnable()
        {
            restartButton.onClick.AddListener(StartGameBtnOnClickHandler);
        }

        private void OnDisable()
        {
            restartButton.onClick.RemoveAllListeners();
        }

        private void StartGameBtnOnClickHandler()
        {
            OnGameRestarted?.Invoke();
        }
    }
}
