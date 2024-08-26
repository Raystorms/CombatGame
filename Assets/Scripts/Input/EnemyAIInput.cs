using CombatGame.CharacterState;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace CombatGame.InputSystem
{
    public class EnemyAIInput : MonoBehaviour
    {
        [SerializeField]
        private CharacterStateMachineControl _characterStateMachineControl;
        private Transform _targetPlayer;

        [SerializeField]
        private float _searchDelay = 1;
        private float _searchDelayTimer = 0;

        [SerializeField]
        private float _attackRange = 1;
        [SerializeField]
        private float _attackCooldown = 5;
        private float _atackCooldownTimer = 0;

        [SerializeField]
        private int _attackWeight = 1;

        [SerializeField]
        private LayerMask _targetLayerMask;

        // Start is called before the first frame update
        void Start()
        {
            _characterStateMachineControl.RegisterForInputPolling(PollInput);
        }

        private void Update()
        {
            if (_searchDelayTimer >= _searchDelay)
            {
                var hits = Physics.OverlapSphere(transform.position, 20, _targetLayerMask);
                if (hits != null && hits.Length > 0)
                {
                    //we just take the first target hit
                    _targetPlayer = hits[0].transform;
                }
                _searchDelayTimer = 0;
            }
            else
            {
                _searchDelayTimer += Time.deltaTime;
            }

            _atackCooldownTimer += Time.deltaTime;
        }

        private CharacterStateInputStruct PollInput()
        {
            var input = new CharacterStateInputStruct();

            if (_targetPlayer != null)
            {
                var myPosition = transform.position;
                var targetPosition = _targetPlayer.position;
                //We want to ignore Y positions as it can cause calculation error
                myPosition.y = 0;
                targetPosition.y = 0;

                var distance = Vector3.Distance(myPosition, targetPosition);

                var moveDirection = (targetPosition - myPosition).normalized;

                if (_atackCooldownTimer >= _attackCooldown && distance <= _attackRange)
                {
                    if (EnemyMastermindSingleton.Instance.RequestToAttack(_attackWeight))
                    {
                        FinishAttack(this.destroyCancellationToken).Forget();
                        input.Attack = true;
                    }
                    _atackCooldownTimer = 0;
                }
                else if (distance > _attackRange)
                {
                    input.MoveDirection = moveDirection;
                }
            }

            return input;
        }

        private async UniTaskVoid FinishAttack(CancellationToken cancellationToken)
        {
            //TODO this is waiting for 1 second of the attack, it's hardcoded, should listen to the attack state finish, but the tate machine Event output is not yet implemented
            await UniTask.Delay(1000, cancellationToken: cancellationToken).SuppressCancellationThrow();

            //canceled or not, we still finish this attack
            EnemyMastermindSingleton.Instance.FinishAttack(_attackWeight);
        }
    }
}