using Data.GameplayConfigure;
using Infrastructure.ProjectStateMachine;
using JetBrains.Annotations;

namespace Infrastructure.ProjectStates
{
    [UsedImplicitly]
    public class GameplayState : IState, IEnterableWithOneArg<GameplayConfigure>, IExitable
    {
        public void OnEnter(GameplayConfigure arg)
        {
        }

        public void OnExit()
        {
        }
    }
}