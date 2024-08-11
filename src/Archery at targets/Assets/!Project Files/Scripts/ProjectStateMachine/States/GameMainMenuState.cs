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

            ShowMainMenu();
        }

        public void OnExit()
        {
            _mainMenuUI.ClearButtons();
        }

        private void ShowMainMenu()
        {
            _mainMenuUI.ClearButtons();
            _mainMenuUI.AddButton("VR режим", ShowVRGamesMenu);
            _mainMenuUI.AddButton("MR режим", ShowMrGamesMenu);
            _mainMenuUI.AddButton("Выход", OnExitButtonClicked);
        }

        private void ShowVRGamesMenu()
        {
            _mainMenuUI.ClearButtons();
            _mainMenuUI.AddButton("На количество поподаний", LoadVRShootingPerNumberHits);
            _mainMenuUI.AddButton("На время", LoadVRShootingForTime);
            _mainMenuUI.AddButton("Бесконечный режим", LoadVRShootingInfinite);
            _mainMenuUI.AddButton("Назад", ShowMainMenu);
        }

        private void ShowMrGamesMenu()
        {
            _mainMenuUI.ClearButtons();
            _mainMenuUI.AddButton("На количество поподаний", LoadMrShootingPerNumberHits);
            _mainMenuUI.AddButton("Назад", ShowMainMenu);
        }

        private void LoadVRShootingPerNumberHits() => LoadGame<VRShootingPerNumberHitsState>();
        private void LoadVRShootingForTime() => LoadGame<VRShootingForTimeState>();
        private void LoadVRShootingInfinite() => LoadGame<VRShootingInfiniteState>();

        private void LoadMrShootingPerNumberHits() => LoadGame<MRShootingPerNumberHitsState>();

        private void LoadGame<T>() where T : IState<GameBootstrap> => Initializer.StateMachine.SwitchState<T>();

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