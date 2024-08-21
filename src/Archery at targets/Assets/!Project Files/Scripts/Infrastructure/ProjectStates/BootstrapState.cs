using Infrastructure.ProjectStateMachine;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure.ProjectStates
{
    [UsedImplicitly]
    public class BootstrapState : IState, IEnterable
    {
        private readonly IProjectStateMachine _projectStateMachine;

        public BootstrapState(IProjectStateMachine projectStateMachine)
        {
            _projectStateMachine = projectStateMachine;
        }

        public void OnEnter()
        {
            LoadBootstrapScene();

            _projectStateMachine.SwitchState<InitializeState>();
        }

        private void LoadBootstrapScene()
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}