using Code.Main;
using UnityEngine;

namespace Code.UI
{
    public class VictoryScreen : MonoBehaviour
    {
        [SerializeField] private VictoryScreenView view;
        
        public void Show(int currentStep, CheckerColor winner)
        {
            view.Show();
            var victoryText = $"{winner} wins on move {currentStep}";
            view.SetVictoryScreenText(victoryText);
        }

        public void Hide()
        {
            view.Hide();
        }

        private void OnEnable()
        {
            view.OnGameRestarted += ShowMainMenu;
        }

        private void OnDisable()
        {
            view.OnGameRestarted -= ShowMainMenu;
        }

        private void ShowMainMenu()
        {
            Hide();
            Core.Instance.ShowMainMenu();
        }
    }
}
