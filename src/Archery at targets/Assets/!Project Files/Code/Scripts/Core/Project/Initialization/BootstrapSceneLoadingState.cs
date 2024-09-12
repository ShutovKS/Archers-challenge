using Infrastructure.Services.ProjectManagement;
using JetBrains.Annotations;
using UnityEngine.SceneManagement;

namespace Core.Project
{
    public class BootstrapSceneLoadingState : IState, IEnterable
    {
        private readonly IProjectManagementService _projectManagementService;

        public BootstrapSceneLoadingState(IProjectManagementService projectManagementService)
        {
            _projectManagementService = projectManagementService;
        }

        public void OnEnter()
        {
            MoveToInstantiateState();
        }

        private void MoveToInstantiateState()
        {
            _projectManagementService.ChangeState<PlayerInstantiateState>();
        }
    }
}