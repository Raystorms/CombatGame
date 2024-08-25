using CombatGame.Util;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

#nullable enable

namespace CombatGame.CharacterState
{
    [CreateAssetMenu(menuName = "CharacterState/AttackState")]
    public class CharacterStateAttacking : CharacterStateBase
    {
        [Serializable]
        public struct HitBoxData
        {
            public Vector3 Size;
            public Vector3 RelativePosition;
            public int HitStopTimeMs;
            public float SpawnTiming;
        }

        [SerializeField]
        private string _attackAnimatorTriggerParam = "Attack";
        [SerializeField]
        private int _attackIndex = 1;
        [SerializeField]
        private string _animatorStateName = "Attack1";
        [SerializeField]
        private float _nextComboStartAt = 0.9f;
        [SerializeField]
        private CharacterStateBase? _nextComboState;
        [SerializeField]
        private float _aimSnapDistance;
        [SerializeField]
        private LayerMask _hitLayerMask;

        [SerializeField]
        private HitBoxData[]? _hitBoxData;

        [Header("Debugs")]
        [SerializeField]
        private RotaryHeart.Lib.PhysicsExtension.PreviewCondition _previewConfig = RotaryHeart.Lib.PhysicsExtension.PreviewCondition.Both;
        [SerializeField]
        private float _previewDuration = 1;


        public override void OnEnterState(CharacterStateMachineControl context)
        {
            context.Animator.SetTrigger(_attackAnimatorTriggerParam);
            context.Animator.SetInteger("AttackIndex", _attackIndex);


            //Set input direction to default transform forward, we want to attack forward
            Vector3 inputDirection = context.transform.forward;
            if (MoveInputGetter.IsMovementInput)
            {
                //Replace with actual input direction if exist
                inputDirection = MoveInputGetter.GetMovementInput(Camera.main);
            }

            //TODO we could make this into a Utility or Helper class so it's not stuck here in the attack script
            //Aim assist calculation
            var hits = RotaryHeart.Lib.PhysicsExtension.Physics.OverlapSphere(context.transform.position, _aimSnapDistance, _hitLayerMask, drawDuration: _previewDuration, preview: _previewConfig);
            if (hits != null && hits.Length > 0)
            {
                //Use the input direction to calculate closest target to the angle
                float closestAngle = float.MaxValue;
                Vector3 finalDirection = Vector3.zero;
                foreach (var hit in hits)
                {
                    var myPosition = context.transform.position;
                    var targetPosition = hit.transform.position;
                    //We want to ignore Y positions as it can cause calculation error
                    myPosition.y = 0;
                    targetPosition.y = 0;

                    var targetDirection = (targetPosition - myPosition).normalized;
                    var angleToTarget = Vector3.Angle(inputDirection, targetDirection);
                    if (angleToTarget < closestAngle)
                    {
                        closestAngle = angleToTarget;
                        finalDirection = targetDirection;
                    }
                }

                //Override the input direction with the final intended direction
                inputDirection = finalDirection;
            }


            context.transform.rotation = Quaternion.LookRotation(inputDirection);

            context.StateCancellationTokenSource = new CancellationTokenSource();
            UpdateAsync(context, context.StateCancellationTokenSource.Token).Forget();
        }

        public async UniTaskVoid UpdateAsync(CharacterStateMachineControl context, CancellationToken cancellationToken)
        {
            bool _attackInputed = false;
            bool _isAnimationTriggered = false;
            int hitboxIndex = 0;


            //IMPROVEMENT NOTE: I'm relying on the animation to start or finish to time my state control, this is not the most optimal way, as sometime you want to allow the combo buffer to be longer or shorter than animation
            while (!_isAnimationTriggered)
            {
                await UniTask.WaitUntil(() => context.Animator.GetCurrentAnimatorStateInfo(0).IsName(_animatorStateName), cancellationToken: cancellationToken);
                _isAnimationTriggered = true;
            }

            while (true)
            {
                //Async cannot do Ref or Out so lets just return the HitboxIndex again
                hitboxIndex = await ProcessHitBox(context, hitboxIndex, cancellationToken);

                if (_nextComboState != null && Input.GetMouseButtonDown(0))
                {
                    _attackInputed = true;
                }

                if (!_attackInputed && !context.Animator.GetCurrentAnimatorStateInfo(0).IsName(_animatorStateName))
                {
                    //if animation is finished & attack is not inputed, we exit this state
                    ExitState(context);
                }
                else if (_attackInputed && context.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _nextComboStartAt)
                {
                    //Move to next state
                    context.ChangeState(_nextComboState);
                }

                await UniTask.NextFrame(cancellationToken: cancellationToken);
            }
        }

        private async UniTask<int> ProcessHitBox(CharacterStateMachineControl context, int hitboxIdex, CancellationToken cancellationToken)
        {
            if (_hitBoxData == null)
            {
                return hitboxIdex;
            }

            if (hitboxIdex < _hitBoxData.Length)
            {
                var hitboxData = _hitBoxData[hitboxIdex];
                if (context.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= hitboxData.SpawnTiming)
                {
                    var hitboxPosition = context.transform.position + (context.transform.rotation * hitboxData.RelativePosition);
                    //TODO Could be optimized with Non-Alloc
                    Debug.Log("Check hitbox");
                    var hit = RotaryHeart.Lib.PhysicsExtension.Physics.OverlapBox(hitboxPosition, hitboxData.Size, context.transform.rotation, _hitLayerMask, drawDuration: _previewDuration, preview: _previewConfig);
                    if (hit != null && hit.Length > 0)
                    {
                        //TODO Proccess hit here
                        context.Animator.SetFloat("SpdMultiplier", 0);
                        await UniTask.Delay(hitboxData.HitStopTimeMs, cancellationToken: cancellationToken);
                        context.Animator.SetFloat("SpdMultiplier", 1);
                    }
                    hitboxIdex++;
                }
            }
            return hitboxIdex;
        }

        public override void OnExitState(CharacterStateMachineControl context)
        {
            context.StateCancellationTokenSource.Cancel();
            context.StateCancellationTokenSource.Dispose();
            context.Animator.SetFloat("SpdMultiplier", 1);

            //We can play sheating animation here
        }
    }
}