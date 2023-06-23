﻿using Common.FSM;
using DG.Tweening;
using Infrastructure.Services.Input;
using Infrastructure.Services.Updating;
using UI.Services.Factories.UI;

namespace Infrastructure.GameFSM.States
{
    public class InitializationState : IState
    {
        private readonly IGameStateMachine _gameStateMachine;
        
        private readonly IInputService _inputService;
        private readonly IUpdater _updater;
        private readonly IUIFactory _uiFactory;
        
        public InitializationState(IGameStateMachine gameStateMachine,
            IInputService inputService,
            IUpdater updater,
            IUIFactory uiFactory)
        {
            _gameStateMachine = gameStateMachine;
            _inputService = inputService;
            _updater = updater;
            _uiFactory = uiFactory;
        }

        public void Enter()
        {
            _inputService.Init();
            _uiFactory.Init();
            DOTween.Init();

            _updater.StartUpdating();
            
            _gameStateMachine.EnterState<MainMenuState>();
        }

        public void Exit()
        {
        }
    }
}