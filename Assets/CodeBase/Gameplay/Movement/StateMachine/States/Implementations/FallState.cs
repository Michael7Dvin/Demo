using System.Collections.Generic;
using Common.Observable;
using Gameplay.Movement.GroundTypeTracking;
using Gameplay.Movement.Rotator;
using Gameplay.Movement.StateMachine.States.Base;
using UnityEngine;

namespace Gameplay.Movement.StateMachine.States.Implementations
{
    public class FallState : MovementState
    {
        private readonly float _verticalSpeed;
        private readonly float _horizontalspeed;
        
        private readonly Transform _camera;
        private readonly IRotator _rotator;

        private readonly IReadOnlyObservable<Vector3> _moveDirection;

        public FallState(float verticalSpeed,
            float horizontalspeed,
            IRotator rotator,
            Transform camera,
            IReadOnlyObservable<Vector3> moveDirection)
        {
            _verticalSpeed = verticalSpeed;
            _horizontalspeed = horizontalspeed;
            
            _camera = camera;
            _rotator = rotator;

            _moveDirection = moveDirection;
        }

        public override MovementStateType Type => MovementStateType.Fall;
        public override float CurrentHorizontalSpeed => _horizontalspeed;
        public override float CurrentVerticalSpeed => _verticalSpeed;

        protected override HashSet<GroundType> CanStartWithGroundTypes { get; } = new()
        {
            GroundType.Air,
        };

        protected override HashSet<GroundType> CanWorkWithGroundTypes { get; } = new()
        {
            GroundType.Air,
        };

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override Vector3 GetMoveVelocty(float deltaTime)
        {
            Vector3 velocity = GetVerticalVelocity(deltaTime) + GetHorizontalVelocity(deltaTime);
            return velocity;
        }

        public override Quaternion GetRotation(Quaternion currentRotation, float deltaTime) =>
            _rotator.GetRotationToDirection(GetMoveVelocty(deltaTime), currentRotation, deltaTime);
        
        private Vector3 GetVerticalVelocity(float deltaTime) => 
            Vector3.down * _verticalSpeed * deltaTime;

        private Vector3 GetHorizontalVelocity(float deltaTime)
        {
            Vector3 velocity = new();
            
            if (_moveDirection.Value != Vector3.zero)
            {
                Vector3 cameraAlignedDirection = AlignDirectionToCameraView(_moveDirection.Value);
                velocity = cameraAlignedDirection * _horizontalspeed * deltaTime;
            }
            
            return velocity;
        }

        private Vector3 AlignDirectionToCameraView(Vector3 direction) => 
            Quaternion.AngleAxis(_camera.rotation.eulerAngles.y, Vector3.up) * direction;
    }
}