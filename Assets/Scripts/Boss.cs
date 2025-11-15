using System;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public event Action<float, float> OnHealthChange;
    
    [SerializeField] private float maxHealth;
    
    public float MaxHealth => maxHealth;

    public float CurrentHealth
    {
        get => _currentHealth;
        private set
        {
            _currentHealth = Mathf.Clamp(value, 0, maxHealth);
            OnHealthChange?.Invoke(_currentHealth, maxHealth);
        }
    }

    private float _currentHealth;

    private void Awake()
    {
        CurrentHealth =  maxHealth;
    }

    public void Parry()
    {
        CurrentHealth -= 10;
    }
}