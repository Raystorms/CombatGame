using CombatGame.CharacterState;
using NaughtyAttributes;
using System;
using UnityEngine;

namespace CombatGame.CharacterStateChanger
{
    [Serializable]
    public abstract class CharacterStateChangerBase : ScriptableObject
    {
        [Expandable]
        [SerializeField]
        internal CharacterStateBase _targetState;

        public abstract bool UpdateCheck(CharacterStateMachineControl context);
        public virtual bool EventCheck(CharacterStateMachineControl context, string eventId) 
        { 
            return false;
        }
    }
}
