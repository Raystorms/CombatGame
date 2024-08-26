using TMPro;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField]
    private HealthComponent _healthComponent;

    [SerializeField]
    private TextMeshProUGUI _healthText;

    private void Start()
    {
        if (_healthComponent != null)
        {
            _healthComponent.OnHealthChanged.AddListener(HealthChanged);
        }
        else
        {
            Debug.LogWarning($"{nameof(PlayerHealthUI)} - {nameof(_healthComponent)} is null");
        }
    }

    private void HealthChanged(int health)
    {
        _healthText.text = health.ToString();
    }


    private void OnDestroy()
    {
        if (_healthComponent != null)
        {
            _healthComponent.OnHealthChanged.RemoveListener(HealthChanged);
        }
    }
}
