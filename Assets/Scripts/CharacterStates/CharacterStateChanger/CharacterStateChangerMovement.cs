using CombatGame.CharacterState;
using CombatGame.Util;
using UnityEngine;

namespace CombatGame.CharacterStateChanger
{
    [CreateAssetMenu(menuName = "CharacterStateChanger/MoveTrigger")]
    public class CharacterStateChangerMovement : CharacterStateChangerBase
    {
        public override bool UpdateCheck(CharacterStateMachineControl context)
        {
            if (MoveInputGetter.IsMovementInput)
            {
                context.ChangeState(_targetState);
                return true;
            }
            return false;
        }
    }
}
