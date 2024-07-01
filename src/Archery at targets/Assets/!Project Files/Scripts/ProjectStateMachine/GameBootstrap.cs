﻿using ProjectStateMachine.Base;
using ProjectStateMachine.States;
using UnityEngine;

namespace ProjectStateMachine
{
    public class GameBootstrap : MonoBehaviour
    {
        private void Start()
        {
            StateMachine = new StateMachine<GameBootstrap>(
                new BootstrapState(this),
                new GameMainMenuState(this),
                new GameVrGameState(this),
                new GameMrGameState(this)
            );

            StateMachine.SwitchState<BootstrapState>();
        }

        public StateMachine<GameBootstrap> StateMachine;
    }
}