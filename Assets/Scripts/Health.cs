using System;
using DG.Tweening;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<float, float> OnHealthChange;
    
    [SerializeField] private float maxHealth;
    [SerializeField] private bool setHealthOnStart;
    
    [SerializeField] private float parryRage;
    [SerializeField] private float hitRage;
    [SerializeField] private float aggroSpeed;
    [SerializeField] private float aggroTime;
    
    [SerializeField] private Transform shakeCamera;
    [SerializeField] private float shakeStrength = 1f;
    
    public float MaxHealth => maxHealth;

    public float CurrentHealth
    {
        get => _currentHealth;
        private set
        {
            _currentHealth = Mathf.Clamp(value, 0, maxHealth * 2);
            OnHealthChange?.Invoke(_currentHealth, maxHealth);
        }
    }

    private float _currentHealth;
    private bool _isInAggroMode;

    private float _saveDifference;

    private void Awake()
    {
        CurrentHealth = setHealthOnStart ? maxHealth : 0;
    }

    public void ChangeHealth(float amount)
    {
        CurrentHealth += amount;
    }

    public void HitParry()
    {
        ChangeHealth(parryRage);
    }

    public void GetHit()
    {
        ChangeHealth(hitRage);

        shakeCamera.transform.DOShakePosition(0.5f, shakeStrength);
    }

    private void Update()
    {
        if (!_isInAggroMode) return;
        
        //CurrentHealth -= Mathf.Clamp(Time.deltaTime * aggroSpeed, 0,  MaxHealth*2);
        CurrentHealth = Mathf.Clamp((1 - _timePassed/aggroTime) * _currentHealthSaver, 0, maxHealth*2);
        
        _timePassed += Time.deltaTime;

        if (CurrentHealth <= 0)
        {
            // ORDER IS IMPORTANT
            maxHealth -= _saveDifference;
            // Trigger update again for health bar change
            ChangeHealth(0);
            _isInAggroMode = false;
        }
    }

    private float _currentHealthSaver;
    private float _timePassed;

    public void SetAggroMode()
    {
        _isInAggroMode = true;
        _currentHealthSaver = CurrentHealth;
        _timePassed = 0;
        _saveDifference = _currentHealth - maxHealth;
    }
}