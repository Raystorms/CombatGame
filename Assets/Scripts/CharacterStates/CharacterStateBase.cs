using UnityEngine;
using CombatGame.Util;
using CombatGame.CharacterStateChanger;
using System;
using NaughtyAttributes;

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

        public virtual void UpdateState(CharacterStateMachineControl context)
        {
            foreach (var trigger in _stateChangeTriggers)
            {
                if (trigger.UpdateCheck(context))
                {
                    return;
                }
            }
        }

        public abstract void OnExitState(CharacterStateMachineControl context);

        public void ExitState(CharacterStateMachineControl context) {
            context.ChangeState(_exitState);
        }
    }
}