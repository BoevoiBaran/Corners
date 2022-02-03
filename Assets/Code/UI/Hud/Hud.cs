using Code.Main;
using UnityEngine;

namespace Code.UI
{
    public class Hud : MonoBehaviour
    {
        [SerializeField] private HudView view;

        public void Show()
        {
            view.Show();
        }

        public void Hide()
        {
            view.Hide();
        }
        
        public void SetStep(int step)
        {
            view.SetStepText($"Step: {step}");
        }
        
        private void OnEnable()
        {
            view.OnGameRestarted += Restart;
            view.OnGameSaved += SaveSession;
        }

        private void OnDisable()
        {
            view.OnGameRestarted -= Restart;
            view.OnGameSaved -= SaveSession;
        }

        private void SaveSession()
        {
            Core.Instance.SaveGame();
        }

        private void Restart()
        {
            Core.Instance.RestartGame();
        }
    }
}

