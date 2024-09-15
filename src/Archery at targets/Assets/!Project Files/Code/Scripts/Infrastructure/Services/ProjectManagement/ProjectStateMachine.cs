using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Factories.ProjectStates;

namespace Infrastructure.Services.ProjectManagement
{
    public class ProjectStateMachine : IProjectManagementService
    {
        private readonly IProjectStatesFactory _projectStatesFactory;
        private IState _currentState;
        private CancellationTokenSource _tickCancellationTokenSource;

        public ProjectStateMachine(IProjectStatesFactory projectStatesFactory)
        {
            _projectStatesFactory = projectStatesFactory;
        }

        public void ChangeState<TState>() where TState : IState
        {
            ExitCurrentState();
            _currentState = _projectStatesFactory.CreateProjectState<TState>();
            EnterCurrentState();
            StartTicking();
        }

        public void ChangeState<TState, T0>(T0 arg) where TState : IState
        {
            ExitCurrentState();
            _currentState = _projectStatesFactory.CreateProjectState<TState>();
            EnterCurrentState(arg);
            StartTicking();
        }

        private void ExitCurrentState()
        {
            if (_currentState is IExitable exitable)
                exitable.OnExit();

            _tickCancellationTokenSource?.Cancel();
        }

        private void EnterCurrentState()
        {
            if (_currentState is IEnterable enterable)
                enterable.OnEnter();
        }

        private void EnterCurrentState<T0>(T0 arg)
        {
            if (_currentState is IEnterableWithArg<T0> enterable)
                enterable.OnEnter(arg);
        }

        private void StartTicking()
        {
            if (_currentState is ITickable tickable)
            {
                _tickCancellationTokenSource = new CancellationTokenSource();
                StartTick(tickable, _tickCancellationTokenSource.Token);
            }
        }

        private async void StartTick(ITickable tickable, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                tickable.Tick();
                await Task.Yield();
            }
        }
    }
}