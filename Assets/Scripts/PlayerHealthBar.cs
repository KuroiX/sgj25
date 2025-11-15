using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Health health;

    private Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        OnOnHealthChange(health.CurrentHealth, health.MaxHealth);
    }

    private void OnEnable()
    {
        health.OnHealthChange += OnOnHealthChange;
    }

    private void OnDisable()
    {
        health.OnHealthChange -= OnOnHealthChange;
    }

    private void OnOnHealthChange(float current, float maxHealth)
    {
        _slider.value = current / maxHealth;
    }
}