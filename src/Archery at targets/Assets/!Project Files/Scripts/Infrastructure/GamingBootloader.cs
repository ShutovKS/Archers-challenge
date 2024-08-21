using Infrastructure.ProjectStateMachine;
using Infrastructure.ProjectStates;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure
{
    [UsedImplicitly]
    public class GamingBootloader : IInitializable
    {
        private readonly IProjectStateMachine _projectStateMachine;

        public GamingBootloader(IProjectStateMachine projectStateMachine)
        {
            _projectStateMachine = projectStateMachine;
        }

        public void Initialize()
        {
            _projectStateMachine.SwitchState<BootstrapState>();
        }
    }
}