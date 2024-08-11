using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.ProjectStateMachine.Base
{
    public class StateMachine<TInitializer>
    {
        public StateMachine(params IState<TInitializer>[] states)
        {
            _states = new Dictionary<Type, IState<TInitializer>>(states.Length);

            foreach (var state in states)
            {
                _states.Add(state.GetType(), state);
            }
        }

        private IState<TInitializer> _currentState;

        private readonly Dictionary<Type, IState<TInitializer>> _states;

        private CancellationTokenSource _tickCancellationTokenSource;

        public void SwitchState<TState>() where TState : IState<TInitializer>
        {
            TryExitPreviousState();

            GetNewState<TState>();

            TryEnterNewState();

            TryTickNewState();
        }

        public void SwitchState<TState, T0>(T0 arg) where TState : IState<TInitializer>
        {
            TryExitPreviousState();

            GetNewState<TState>();

            TryEnterNewState<T0>(arg);

            TryTickNewState();
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
            if (_currentState is IEnterableWithOneArg<T0> enterable)
            {
                enterable.OnEnter(arg);
            }
        }

        private void GetNewState<TState>() where TState : IState<TInitializer>
        {
            var newState = GetState<TState>();
            _currentState = newState;
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

        private TState GetState<TState>() where TState : IState<TInitializer>
        {
            return (TState)_states[typeof(TState)];
        }
    }
}