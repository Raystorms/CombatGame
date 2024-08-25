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

        [SerializeField]
        private bool _enableDebug;

        [ShowIf(nameof(_enableDebug))]
        [SerializeField]
        private string _debugEvent = "Flinch";

        //this input struct is accessible internally, it's fed from outside source by registering to the delegate
        internal CharacterStateInputStruct _characterInput;
        private PollInput _inputPollDelegate;

        public CancellationTokenSource StateCancellationTokenSource;

        public delegate CharacterStateInputStruct PollInput();

        private void Start()
        {
            _currentState = _startingState;
            _currentState.OnEnterState(this);
        }

        private void Update()
        {
            //poll input from any outside provider, if none is provided, then just set default to reset it
            _characterInput = _inputPollDelegate?.Invoke() ?? default;

            var movementVector = _currentState.UpdateState(this);
            movementVector.y = -_characterGravity * Time.deltaTime;
            _characterController.Move(movementVector);
        }

        internal void ChangeState(CharacterStateBase characterState)
        {
            _currentState.OnExitState(this);
            _currentState = characterState;
            _currentState.OnEnterState(this);
        }

        public void TriggerEvent(string eventId)
        {
            _currentState.TriggerEvent(this, eventId);
        }

        [ShowIf(nameof(_enableDebug))]
        [Button]
        public void DebugTriggerEvent()
        {
            TriggerEvent(_debugEvent);
        }


        public void RegisterForInputPolling(PollInput pollInput)
        {
            _inputPollDelegate = pollInput;
        }

        private void OnDestroy()
        {
            StateCancellationTokenSource?.Cancel();
            StateCancellationTokenSource?.Dispose();
        }
    }
}