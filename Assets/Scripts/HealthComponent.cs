using CombatGame.CharacterState;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField]
    private CharacterStateMachineControl _characterStateMachineControl;

    [SerializeField]
    private CharacterController _characterController;

    [SerializeField]
    private string _flinchEventId = "Flinch";

    [SerializeField]
    private string _deadEventId = "Dead";

    [SerializeField]
    private int _health = 100;

    [SerializeField]
    private int _destroyAfterDeadDelayMs = 2000;

    [SerializeField]
    private UnityEvent<int> _onHealthChanged;
    public UnityEvent<int> OnHealthChanged => _onHealthChanged;


    [SerializeField]
    private UnityEvent _onDeath;
    public UnityEvent OnDeath => _onDeath;

    public void GetDamaged(int value)
    {
        _health -= value;
        _characterStateMachineControl.TriggerEvent(_flinchEventId);
        _onHealthChanged?.Invoke(_health);
        if (_health <= 0)
        {
            _characterStateMachineControl.TriggerEvent(_deadEventId);
            _characterController.enabled = false;
            OnDeath?.Invoke();
            DelayDeathDestroy(_destroyAfterDeadDelayMs, this.destroyCancellationToken).Forget();
        }
    }

    private async UniTaskVoid DelayDeathDestroy(int miliSecond, CancellationToken cancellationToken)
    {
        await UniTask.Delay(miliSecond, cancellationToken: cancellationToken);
        Destroy(gameObject);
    }
}
