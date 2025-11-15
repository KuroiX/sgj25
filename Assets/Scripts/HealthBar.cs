using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;
    
    private float _startHealth;
    private bool _startHealthSet;

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
        if (!_startHealthSet) 
        {
            _startHealth = maxHealth;
            _startHealthSet = true;
        }
        
        _slider.value = current / _startHealth;
    }
}