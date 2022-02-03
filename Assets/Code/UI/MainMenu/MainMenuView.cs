using System;
using Code.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class MainMenuView : MonoBehaviour
    {
        public event Action OnNewGameStarted;
        public event Action OnSavedGameStarted;
        public event Action<int> OnStepTypeChanged;
        
        [SerializeField] private Button startNewGameBtn;
        [SerializeField] private Button loadSavedGameBtn;
        [SerializeField] private Dropdown dropdown;

        public void Show()
        {
            gameObject.SetSafeActive(true);
        }

        public void Hide()
        {
            gameObject.SetSafeActive(false);
        }
        
        private void OnEnable()
        {
            startNewGameBtn.onClick.AddListener(StartGameBtnOnClickHandler);
            loadSavedGameBtn.onClick.AddListener(LoadGameBtnOnClickHandler);
            dropdown.onValueChanged.AddListener(OnWalkTypeChangedHandler);
        }

        private void OnDisable()
        {
            startNewGameBtn.onClick.RemoveAllListeners();
            loadSavedGameBtn.onClick.RemoveAllListeners();
            dropdown.onValueChanged.RemoveAllListeners();
        }

        private void StartGameBtnOnClickHandler()
        {
            OnNewGameStarted?.Invoke();
        }
        
        private void LoadGameBtnOnClickHandler()
        {
            OnSavedGameStarted?.Invoke();
        }

        private void OnWalkTypeChangedHandler(int stepType)
        {
            OnStepTypeChanged?.Invoke(stepType);
        }
    }
}
