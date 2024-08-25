using System;
using UnityEngine;

namespace CombatGame.CharacterState
{
    [Serializable]
    [CreateAssetMenu(menuName = "CharacterState/IdleState")]
    public class CharacterStateIdle : CharacterStateBase
    {
        public override void OnEnterState(CharacterStateMachineControl context)
        {
            //Set animation param to idling
        }


        public override void OnExitState(CharacterStateMachineControl context)
        {
        }
    }
}
