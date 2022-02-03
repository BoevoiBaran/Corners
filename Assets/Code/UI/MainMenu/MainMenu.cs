using Code.Main;
using Code.Main.StepLogicStrategy;
using Code.Session;
using UnityEngine;

namespace Code.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private MainMenuView view;

        private StepType _currentSelected = StepType.Straight;
        
        public void Show()
        {
            view.Show();
        }

        public void Hide()
        {
            view.Hide();
        }

        private void OnEnable()
        {
            view.OnNewGameStarted += StartSession;
            view.OnSavedGameStarted += LoadSession;
            view.OnStepTypeChanged += ChangeStepType;
        }

        private void OnDisable()
        {
            view.OnNewGameStarted -= StartSession;
            view.OnSavedGameStarted -= LoadSession;
            view.OnStepTypeChanged -= ChangeStepType;
        }

        private void ChangeStepType(int stepType)
        {
            _currentSelected = (StepType) stepType;
        }

        private void StartSession()
        {
            Core.Instance.StartSession(new SessionContext
            {
              SceneName  = "Corners",
              StepType = _currentSelected
            });
        }

        private void LoadSession()
        {
            Core.Instance.LoadSession(new SessionContext
            {
                SceneName  = "Corners",
            });
        }
    }
}
