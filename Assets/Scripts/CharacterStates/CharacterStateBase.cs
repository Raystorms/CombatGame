using UnityEngine;
using CombatGame.Util;
using CombatGame.CharacterStateChanger;
using System;
using NaughtyAttributes;
using static UnityEditor.Timeline.TimelinePlaybackControls;

namespace CombatGame.CharacterState
{
    [Serializable]
    public abstract class CharacterStateBase : ScriptableObject
    {
        //This is the default state change on inputs, not the best, Input checking could also be modularize into a Scriptable object & reused
        [SerializeField]
        internal CharacterStateBase _exitState;

        [SerializeField]
        [Expandable()]
        internal CharacterStateChangerBase[] _stateChangeTriggers;

        public abstract void OnEnterState(CharacterStateMachineControl context);

        public virtual Vector3 UpdateState(CharacterStateMachineControl context)
        {
            foreach (var trigger in _stateChangeTriggers)
            {
                if (trigger.UpdateCheck(context))
                {
                    return Vector3.zero;
                }
            }
            return Vector3.zero;
        }

        public abstract void OnExitState(CharacterStateMachineControl context);


        public void TriggerEvent(CharacterStateMachineControl context, string eventId)
        {
            //TODO Improvement, this will call event on all the State Changer, not optmila when there is a lot of state changer, we could do a dynamic detection to register a list of State Changer for each event
            foreach (var trigger in _stateChangeTriggers)
            {
                if (trigger.EventCheck(context, eventId))
                {
                    return;
                }
            }
        }

        protected void ExitState(CharacterStateMachineControl context) {
            context.ChangeState(_exitState);
        }
    }
}