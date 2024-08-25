using NaughtyAttributes;
using System.Threading;
using UnityEngine;

namespace CombatGame.CharacterState
{
    public class CharacterStateMachineControl : MonoBehaviour
    {
        [SerializeField]
        private CharacterController _characterController;
        public CharacterController CharacterController => _characterController;

        [SerializeField]
        private Animator _animator;
        public Animator Animator => _animator;

        [SerializeField]
        private float _characterGravity = 20;

        [SerializeField]
        [Expandable]
        private CharacterStateBase _startingState;

        [SerializeField]
        private CharacterStateBase _currentState;

        public CancellationTokenSource StateCancellationTokenSource;

        private void Start()
        {
            _currentState = _startingState;
            _currentState.OnEnterState(this);
        }

        private void Update()
        {
            _characterController.Move(new Vector3(0, -_characterGravity, 0));
            _currentState.UpdateState(this);
        }

        public void ChangeState(CharacterStateBase characterState)
        {
            _currentState.OnExitState(this);
            _currentState = characterState;
            _currentState.OnEnterState(this);
        }
    }
}