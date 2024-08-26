
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace CombatGame.CharacterState
{
    [CreateAssetMenu(menuName = "CharacterState/FlinchState")]
    public class CharacterStateFlinch : CharacterStateBase
    {
        [SerializeField]
        private int _flinchDurationMs = 300;

        [SerializeField]
        public string _animationBoolName = "Flinch";

        public override void OnEnterState(CharacterStateMachineControl context)
        {
            context.StateCancellationTokenSource = new CancellationTokenSource();
            UpdateAsync(context, context.StateCancellationTokenSource.Token).Forget();
            context.Animator.SetBool(_animationBoolName, true);
        }

        public async UniTaskVoid UpdateAsync(CharacterStateMachineControl context, CancellationToken cancellationToken)
        {
            await UniTask.Delay(_flinchDurationMs, cancellationToken: cancellationToken);
            ExitState(context);
        }

        public override void OnExitState(CharacterStateMachineControl context)
        {
            context.StateCancellationTokenSource.Cancel();
            context.StateCancellationTokenSource.Dispose();
            context.Animator.SetBool(_animationBoolName, false);
        }
    }
}