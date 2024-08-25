using CombatGame.CharacterState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField]
    private CharacterStateMachineControl _characterStateMachineControl;

    [SerializeField]
    private string _eventName = "Flinch";

    [SerializeField]
    private int _health = 100;

    public void GetDamaged(int value)
    {
        _health -= value;
        _characterStateMachineControl.TriggerEvent(_eventName);
        if (_health <= 0)
        { 
            Destroy(gameObject);
        }
    }
}
