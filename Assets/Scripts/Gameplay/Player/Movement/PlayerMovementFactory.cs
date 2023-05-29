﻿using Gameplay.Movement;
using Gameplay.Movement.GroundSpherecasting;
using Gameplay.Movement.GroundTypeTracking;
using Gameplay.Movement.SlopeCalculation;
using Gameplay.Movement.SlopeMovement;
using Gameplay.Movement.States.Implementations;
using Infrastructure.Services.Configuration;
using Infrastructure.Services.Input;
using Infrastructure.Services.Logger;
using Infrastructure.Services.Updater;
using UnityEngine;

namespace Gameplay.Player.Movement
{
    public class PlayerMovementFactory : IPlayerMovementFactory
    {
        private readonly PlayerMovementConfig _config;

        private readonly IGroundSpherecasterFactory _groundSpherecasterFactory;
        
        private readonly IUpdater _updater;
        private readonly IInputService _input;
        private readonly ICustomLogger _logger;

        public PlayerMovementFactory(IConfigProvider configProvider,
            IUpdater updater,
            IInputService input,
            ICustomLogger logger)
        {
            _config = configProvider.GetForPlayer().MovementConfig;
            
            _updater = updater;
            _input = input;
            _logger = logger;

            _groundSpherecasterFactory = new GroundSpherecasterFactory(_updater);
        }

        public IPlayerMovement Create(Transform parent, CharacterController characterController, Transform camera)
        {
            IGroundSpherecaster groundSpherecaster = CreateGroundSpherecaster(parent);
            ISlopeCalculator slopeCalculator = CreateSlopeCalculator(groundSpherecaster);
            IGroundTypeTracker groundTypeTracker = 
                CreateGroundTypeTracker(groundSpherecaster);

            ISlopeSlideMovement slopeSlideMovement =
                new SlopeSlideMovement(_config.SlopeSlideSpeed, _config.MinSlopeAngle, slopeCalculator);
            
            IMovementStateMachine movementStateMachine = 
                CreateMovementStateMachine(camera, groundTypeTracker, slopeSlideMovement);

            PlayerMovement movement = new(movementStateMachine,
                characterController,
                groundSpherecaster,
                groundTypeTracker,
                slopeCalculator,
                slopeSlideMovement,
                _updater,
                _input);
            
            return movement;
        }

        private IGroundSpherecaster CreateGroundSpherecaster(Transform parent)
        {
            return _groundSpherecasterFactory.Create(parent, _config.GroundSphereCastingPointPrefab,
                _config.GroundSphereCastingSphereRadius, _config.GroundSphereCastingDistance);
        }
        
        private IGroundTypeTracker CreateGroundTypeTracker(IGroundSpherecaster groundSpherecaster) => 
            new GroundTypeTracker(groundSpherecaster);

        private MovementStateMachine CreateMovementStateMachine(Transform camera,
            IGroundTypeTracker groundTypeTracker, ISlopeSlideMovement slopeSlideMovement)
        {
            JogState jogState = 
                new(_config.JogSpeed, _config.JogAntiBumpSpeed, slopeSlideMovement, camera, _input);

            FallState fallState = 
                new(_config.FallVerticalSpeed, _config.FallHorizontalSpeed, camera, _input);

            JumpState jumpState = 
                new(_config.JumpYSpeedToTimeCurve, _config.JumpHorizontalSpeed, camera, _input);
            
            IMovementStateProvider stateProvider = 
                new MovementStateProvider(jogState, fallState, _logger);

            stateProvider.AddState(jogState);
            stateProvider.AddState(fallState);
            stateProvider.AddState(jumpState);

            MovementStateMachine movementStateMachine = new(stateProvider, groundTypeTracker);
            
            return movementStateMachine;
        }

        private ISlopeCalculator CreateSlopeCalculator(IGroundSpherecaster groundSpherecaster) => 
            new SlopeCalculator(groundSpherecaster);
    }
}