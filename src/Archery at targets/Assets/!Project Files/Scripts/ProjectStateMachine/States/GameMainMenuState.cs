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

        private MainMenuUI _mainMenuUI;

        public void OnEnter()
        {
            var loadSceneAsync = SceneManager.LoadSceneAsync("MainMenu");
            loadSceneAsync!.completed += OnSceneLoaded;
        }

        private void OnSceneLoaded(AsyncOperation asyncOperation)
        {
            _mainMenuUI = Object.FindFirstObjectByType<MainMenuUI>();
            _mainMenuUI.OnVrGameButtonClicked += OnVrGameButtonClicked;
            _mainMenuUI.OnMrGameButtonClicked += OnMrGameButtonClicked;
            _mainMenuUI.OnExitButtonClicked += OnExitButtonClicked;
        }

        public void OnExit()
        {
            _mainMenuUI.OnVrGameButtonClicked -= OnVrGameButtonClicked;
            _mainMenuUI.OnMrGameButtonClicked -= OnMrGameButtonClicked;
            _mainMenuUI.OnExitButtonClicked -= OnExitButtonClicked;
        }

        private void OnVrGameButtonClicked()
        {
            Initializer.StateMachine.SwitchState<VRShootingPerNumberHitsState>();
        }

        private void OnMrGameButtonClicked()
        {
            Initializer.StateMachine.SwitchState<MRShootingPerNumberHitsState>();
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