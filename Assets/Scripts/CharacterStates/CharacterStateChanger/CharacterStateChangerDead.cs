using CombatGame.CharacterState;
using UnityEngine;

namespace CombatGame.CharacterStateChanger
{
    [CreateAssetMenu(menuName = "CharacterStateChanger/DeadTrigger")]
    public class CharacterStateChangerDead : CharacterStateChangerBase
    {
        public override bool UpdateCheck(CharacterStateMachineControl context)
        {
            if (context._characterInput.Dash)
            {
                context.ChangeState(_targetState);
                return true;
            }
            return false;
        }
    }
}