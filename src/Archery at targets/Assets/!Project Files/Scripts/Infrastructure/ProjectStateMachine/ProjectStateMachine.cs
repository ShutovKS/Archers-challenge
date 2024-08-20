using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Zenject;

namespace Infrastructure.ProjectStateMachine
{
    [UsedImplicitly]
    public class ProjectStateMachine : IProjectStateMachine
    {
        private IState _currentState;
        private readonly DiContainer _container;
        private CancellationTokenSource _tickCancellationTokenSource;

        public ProjectStateMachine(DiContainer container)
        {
            _container = container;
        }

        public void SwitchState<TState>() where TState : IState
        {
            TryExitPreviousState();

            GetNewState<TState>();

            TryEnterNewState();

            TryTickNewState();
        }

        public void SwitchState<TState, T0>(T0 arg) where TState : IState
        {
            TryExitPreviousState();

            GetNewState<TState>();

            TryEnterNewState(arg);

            TryTickNewState();
        }

        public IState GetCurrentState()
        {
            return _currentState;
        }

        public Type GetCurrentStateType()
        {
            return _currentState?.GetType();
        }

        private void TryExitPreviousState()
        {
            if (_currentState is IExitable exitable)
            {
                exitable.OnExit();
            }

            _tickCancellationTokenSource?.Cancel();
        }

        private void TryEnterNewState()
        {
            if (_currentState is IEnterable enterable)
            {
                enterable.OnEnter();
            }
        }

        private void TryEnterNewState<T0>(T0 arg)
        {
            if (_currentState is IEnterableWithArg<T0> enterable)
            {
                enterable.OnEnter(arg);
            }
        }

        private void GetNewState<TState>() where TState : IState
        {
            _currentState = _container.Instantiate<TState>();
        }

        private void TryTickNewState()
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