using ProjectStateMachine.Base;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectStateMachine.States
{
    public class GameMainMenuState : IState<GameBootstrap>, IEnterable, IExitable
    {
        public GameBootstrap Initializer { get; }

        public GameMainMenuState(GameBootstrap initializer)
        {
            Initializer = initializer;
        }

        public void OnEnter()
        {
            var loadSceneAsync = SceneManager.LoadSceneAsync("MainMenu");
            loadSceneAsync!.completed += OnSceneLoaded;
        }

        private void OnSceneLoaded(AsyncOperation asyncOperation)
        {
            var mainMenuUI = Object.FindFirstObjectByType<MainMenuUI>();
            mainMenuUI.OnVrGameButtonClicked += OnVrGameButtonClicked;
            mainMenuUI.OnMrGameButtonClicked += OnMrGameButtonClicked;
            mainMenuUI.OnExitButtonClicked += OnExitButtonClicked;
        }

        public void OnExit()
        {
        }

        private void OnVrGameButtonClicked()
        {
            // Initializer.StateMachine.SwitchState<GameVrGameState>();
        }

        private void OnMrGameButtonClicked()
        {
            // Initializer.StateMachine.SwitchState<GameMrGameState>();
        }

        private void OnExitButtonClicked()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}