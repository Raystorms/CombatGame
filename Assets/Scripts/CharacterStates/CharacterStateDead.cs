using UnityEngine;

namespace CombatGame.CharacterState
{
    [CreateAssetMenu(menuName = "CharacterState/DeadState")]
    public class CharacterStateDead : CharacterStateBase
    {

        [SerializeField]
        public string _animationBoolName = "Dead";

        public override void OnEnterState(CharacterStateMachineControl context)
        {
            context.Animator.SetTrigger(_animationBoolName);

        }

        public override void OnExitState(CharacterStateMachineControl context)
        {
        }
    }
}
