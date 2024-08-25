using CombatGame.CharacterState;
using UnityEngine;

namespace CombatGame.CharacterStateChanger
{
    [CreateAssetMenu(menuName = "CharacterStateChanger/EventTrigger")]
    public class CharacterStateChangerEventTrigger : CharacterStateChangerBase
    {
        [SerializeField]
        private string _eventId = "Flinch";

        public override bool UpdateCheck(CharacterStateMachineControl context)
        {
            return false;
        }

        public override bool EventCheck(CharacterStateMachineControl context, string eventId)
        {
            if (eventId == _eventId)
            {
                context.ChangeState(_targetState);
                return true;
            }
            return false;
        }
    }
}