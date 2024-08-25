using CombatGame.CharacterState;
using System;
using UnityEngine;

namespace CombatGame.CharacterStateChanger
{
    [Serializable]
    [CreateAssetMenu(menuName = "CharacterStateChanger/DashTrigger")]
    public class ChracterStateChangerDash : CharacterStateChangerBase
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