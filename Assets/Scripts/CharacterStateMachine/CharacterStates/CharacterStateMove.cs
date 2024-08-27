using CombatGame.Util;
using UnityEngine;

namespace CombatGame.CharacterState
{
    [CreateAssetMenu(menuName = "CharacterState/MoveState")]
    public class CharacterStateMove : CharacterStateBase
    {
        [SerializeField]
        public float _moveSpeed = 5;
        [SerializeField]
        public string _animtionBoolName = "Running";

        public override void OnEnterState(CharacterStateMachineControl context)
        {
            context.Animator.SetBool(_animtionBoolName, true);
        }

        public override void OnExitState(CharacterStateMachineControl context)
        {
            context.Animator.SetBool(_animtionBoolName, false);
        }

        public override Vector3 UpdateState(CharacterStateMachineControl context)
        {
            Vector3 moveDirection = default(Vector3);
            if (context._characterInput.MoveDirection != default)
            {
                var _moveSpeedDelta = _moveSpeed * Time.deltaTime;
                var movementInput = context._characterInput.MoveDirection;
                moveDirection = new Vector3(movementInput.x * _moveSpeedDelta, 0, movementInput.z * _moveSpeedDelta);
                context.transform.rotation = Quaternion.LookRotation(movementInput);
            }
            else
            {
                ExitState(context);
            }

            base.UpdateState(context);
            return moveDirection;
        }
    }
}