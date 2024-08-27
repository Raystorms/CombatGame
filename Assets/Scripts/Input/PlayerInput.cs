using CombatGame.CharacterState;
using CombatGame.Util;
using UnityEngine;

namespace CombatGame.InputSystem {
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField]
        private CharacterStateMachineControl _characterStateMachineControl;

        // Start is called before the first frame update
        void Start()
        {
            _characterStateMachineControl.RegisterForInputPolling(PollInput);
        }

        private CharacterStateInputStruct PollInput() 
        { 
            var input = new CharacterStateInputStruct();
        
            input.Attack = Input.GetMouseButtonDown(0);
            input.Dash = Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.LeftShift);
            input.MoveDirection = MoveInputGetter.IsMovementInput ? MoveInputGetter.GetMovementInput(Camera.main) : default;

            return input;
        }
    }
}