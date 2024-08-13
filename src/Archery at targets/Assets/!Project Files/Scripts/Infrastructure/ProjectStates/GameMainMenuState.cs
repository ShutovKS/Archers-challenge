using Infrastructure.Services.ProjectStateMachine;
using JetBrains.Annotations;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.ProjectStates
{
    [UsedImplicitly]
    public class GameMainMenuState : IState, IEnterable, IExitable
    {
        private readonly IProjectStateMachineService _projectStateMachine;

        private MainMenuUI _mainMenuUI;

        public GameMainMenuState(IProjectStateMachineService projectStateMachine)
        {
            _projectStateMachine = projectStateMachine;
        }

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
            _mainMenuUI.AddButton("Выход", ExitFromGame);
        }

        private void ShowVRGamesMenu()
        {
            _mainMenuUI.ClearButtons();
            // _mainMenuUI.AddButton("На количество попаданий", LoadGame<VRShootingPerNumberHitsState>);
            // _mainMenuUI.AddButton("На время", LoadGame<VRShootingForTimeState>);
            // _mainMenuUI.AddButton("Бесконечный режим", LoadGame<VRShootingInfiniteState>);
            _mainMenuUI.AddButton("Назад", ShowMainMenu);
        }

        private void ShowMrGamesMenu()
        {
            _mainMenuUI.ClearButtons();
            // _mainMenuUI.AddButton("На количество попаданий", LoadGame<MRShootingPerNumberHitsState>);
            _mainMenuUI.AddButton("Назад", ShowMainMenu);
        }

        // private void LoadGame<T>() where T : IState =>
        // _projectStateMachine.SwitchState<LoadScenesState, string>(typeof(T).Name);

        private void ExitFromGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}