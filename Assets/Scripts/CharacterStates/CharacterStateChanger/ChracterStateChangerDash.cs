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
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetMouseButtonDown(1)) 
            {
                context.ChangeState(_targetState);
                return true;
            }
            return false;
        }
    }
}