using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Code.Exceptions;
using Code.Session;
using Code.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Main
{
    public class Core : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;
        [SerializeField] private SessionLoader sessionLoader;
        
        public static Core Instance { get; private set; }

        private Session _session;

        private IEnumerator Start()
        {
            if (Instance != null)
            {
                throw new CoreException("Core already created");
            }
                
            Instance = this;
            
            DontDestroyOnLoad(this);
            
            var cachedYield = new WaitForSeconds(0.5f);
            
            uiManager.LoadingScreen.Show();
            uiManager.LoadingScreen.SetLoadingProgress(0.0f);

            yield return cachedYield;
            
            uiManager.Initialize();
            
            uiManager.LoadingScreen.SetLoadingProgress(0.3f);
            yield return cachedYield;
            
            uiManager.LoadingScreen.SetLoadingProgress(0.8f);
            yield return cachedYield;
            
            uiManager.LoadingScreen.SetLoadingProgress(1.0f);
            uiManager.LoadingScreen.Hide();

            ShowMainMenu();
            
            yield return null;
        }

        #region UI

        public void ShowMainMenu()
        {
            var mainMenu = uiManager.GetWindow<MainMenu>();
            mainMenu.Show();
        }
        
        private void HideMainMenu()
        {
            var mainMenu = uiManager.GetWindow<MainMenu>();
            mainMenu.Hide();
        }
        
        private void UpdateHud(int step)
        {
            var hud = uiManager.GetWindow<Hud>();
            hud.SetStep(step);
        }

        #endregion

        #region Session

        public void StartSession(SessionContext context)
        {
            HideMainMenu();
            StartCoroutine(GoToSessionAsync(context));
        }
        
        public void LoadSession(SessionContext context)
        {
            var path = Path.Combine(Application.dataPath);
            var fileName = "saved_session.dat";
            var filePath = Path.Combine(path, fileName);
            
            if (!File.Exists(filePath))
            {
                return;
            }
            
            BinaryFormatter formatter = new BinaryFormatter();
            
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                SessionState state = null;
                try
                {
                    state = (SessionState) formatter.Deserialize(fs);
                }
                catch (Exception e)
                {
                    Debug.LogError( $"Can't deserialize. Exception:{e}");    
                }
                
                context.State = state;
            }
            
            StartCoroutine(GoToSessionAsync(context));
            HideMainMenu();
        }

        public void FinishSession(int currentStep, CheckerColor currentPlayer)
        {
            var hud = uiManager.GetWindow<Hud>();
            hud.Hide();
            
            var victoryScreen = uiManager.GetWindow<VictoryScreen>();
            victoryScreen.Show(currentStep, currentPlayer);
            
            StartCoroutine(UnloadSession());
        }
        
        private IEnumerator GoToSessionAsync(SessionContext context)
        {
            uiManager.LoadingScreen.Show();
            
            sessionLoader.StartSession(context, RegisterCurrentSession);
            
            while (!sessionLoader.SessionReady)
            {
                yield return null;
            }
            
            uiManager.LoadingScreen.SetLoadingProgress(1.0f);
            uiManager.LoadingScreen.Hide();
            
            var hud = uiManager.GetWindow<Hud>();
            hud.Show();
        }

        private IEnumerator UnloadSession()
        {
            yield return SceneManager.UnloadSceneAsync(sessionLoader.SceneName);
            
            yield return Resources.UnloadUnusedAssets();

            _session.RemoveAllListeners();
            _session = null;
            
            GC.Collect();
        }

        private void RegisterCurrentSession(Session session)
        {
            _session = session;
            _session.OnStepUpdate += UpdateHud;
        }

        #endregion

        public void SaveGame()
        {
            var sessionState = _session.GetSessionState();
            var path = Path.Combine(Application.dataPath);
            var fileName = "saved_session.dat";
            var filePath = Path.Combine(path, fileName);

            BinaryFormatter formatter = new BinaryFormatter();
            
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, sessionState);
            }
        }
        
        public void RestartGame()
        {
            var hud = uiManager.GetWindow<Hud>();
            hud.Hide();
            
            StartCoroutine(UnloadSession());
            ShowMainMenu();
        }
    }
}
