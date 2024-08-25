using CombatGame.CharacterState;
using UnityEngine;

namespace CombatGame.CharacterStateChanger
{
    [CreateAssetMenu(menuName = "CharacterStateChanger/AttackTrigger")]
    public class CharacterStateChangerAttack : CharacterStateChangerBase
    {
        public override bool UpdateCheck(CharacterStateMachineControl context)
        {
            if (Input.GetMouseButtonDown(0))
            {
                context.ChangeState(_targetState);
                return true;
            }
            return false;
        }
    }
}