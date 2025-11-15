using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;

    private Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        OnHealthChange(health.CurrentHealth, health.MaxHealth);
    }

    private void OnEnable()
    {
        health.OnHealthChange += OnHealthChange;
    }

    private void OnDisable()
    {
        health.OnHealthChange -= OnHealthChange;
    }

    private void OnHealthChange(float current, float maxHealth)
    {
        _slider.value = current / maxHealth;
    }
}