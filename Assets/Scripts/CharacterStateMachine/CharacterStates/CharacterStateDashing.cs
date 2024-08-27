using CombatGame.Util;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace CombatGame.CharacterState
{
    [CreateAssetMenu(menuName = "CharacterState/DashState")]
    public class CharacterStateDashing : CharacterStateBase
    {
        [SerializeField]
        private float _dashDistance = 5;
        [SerializeField]
        private float _dashSpeed = 10;
        [SerializeField]
        private AnimationCurve _speedCurve;
        [SerializeField]
        private CharacterStateBase _sprintingState;

        public override void OnEnterState(CharacterStateMachineControl context)
        {
            context.Animator.SetBool("Dashing", true);

            context.StateCancellationTokenSource = new CancellationTokenSource();
            if (MoveInputGetter.IsMovementInput)
            {
                var movementInput = MoveInputGetter.GetMovementInput(Camera.main);
                context.transform.rotation = Quaternion.LookRotation(movementInput);
            }

            context.StateCancellationTokenSource = new CancellationTokenSource();
            UpdateAsync(context, context.StateCancellationTokenSource.Token).Forget();
        }

        public async UniTaskVoid UpdateAsync(CharacterStateMachineControl context, CancellationToken cancellationToken)
        {
            var direction = context.transform.transform.forward;
            direction.y = 0;

            float movementDelta = 0;

            //TODO : improvement, there should be a deceleration for better looking animation
            while (movementDelta < _dashDistance)
            {
                var prevPosition = context.transform.position;
                prevPosition.y = 0;

                //Evaluating speed curve
                var distanceProgress = movementDelta / _dashDistance;
                var dashSpeed = _dashSpeed * _speedCurve.Evaluate(distanceProgress);
                var _moveSpeedDelta = dashSpeed * Time.deltaTime;
                var moveVector = direction.normalized * _moveSpeedDelta;

                context.CharacterController.Move(moveVector);

                var currentPosition = context.transform.position;
                currentPosition.y = 0;

                //TODO : Not the most accurate way to get dash distance, this will overshoot it by a bit, but it's fine for now
                movementDelta += Mathf.Abs((currentPosition - prevPosition).magnitude);

                await UniTask.NextFrame(cancellationToken: cancellationToken);
            }

            if (MoveInputGetter.IsMovementInput)
            {
                context.ChangeState(_sprintingState);
                return;
            }

            ExitState(context);
        }

        public override void OnExitState(CharacterStateMachineControl context)
        {
            context.Animator.SetBool("Dashing", false);
            context.StateCancellationTokenSource.Cancel();
            context.StateCancellationTokenSource.Dispose();
            context.StateCancellationTokenSource = null;
        }
    }
}