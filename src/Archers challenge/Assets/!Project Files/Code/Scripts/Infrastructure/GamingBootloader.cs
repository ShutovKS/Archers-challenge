#region

using Core.Project;
using Infrastructure.Services.ProjectManagement;
using JetBrains.Annotations;
using Zenject;

#endregion

namespace Infrastructure
{
    [UsedImplicitly]
    public class GamingBootloader : IInitializable
    {
        private readonly IProjectManagementService _projectManagementService;

        public GamingBootloader(IProjectManagementService projectManagementService)
        {
            _projectManagementService = projectManagementService;
        }

        public void Initialize()
        {
            _projectManagementService.ChangeState<BootstrapState>();
        }
    }
}