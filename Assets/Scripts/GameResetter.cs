using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResetter : MonoBehaviour
{
    [SerializeField]
    private HealthComponent _healthComponent;

    [SerializeField]
    private GameObject _resetText;

    private void Start()
    {
        if (_healthComponent != null)
        {
            _healthComponent.OnDeath.AddListener(OnDeath);
        }
        else
        {
            Debug.LogWarning($"{nameof(PlayerHealthUI)} - {nameof(_healthComponent)} is null");
        }
    }

    private void OnDeath()
    {
        _resetText.gameObject.SetActive(true);
        PollForResetKey(destroyCancellationToken).Forget();
    }


    private async UniTaskVoid PollForResetKey(CancellationToken cancellationToken)
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetScene();
            }
            await UniTask.NextFrame();
        }
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    private void OnDestroy()
    {
        if (_healthComponent != null)
        {
            _healthComponent.OnDeath.RemoveListener(OnDeath);
        }
    }
}
