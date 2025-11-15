using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Boss boss;

    private Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        BossOnOnHealthChange(boss.CurrentHealth, boss.MaxHealth);
    }

    private void OnEnable()
    {
        boss.OnHealthChange += BossOnOnHealthChange;
    }

    private void OnDisable()
    {
        boss.OnHealthChange -= BossOnOnHealthChange;
    }

    private void BossOnOnHealthChange(float current, float maxHealth)
    {
        Debug.Log($"new health {current} of  {maxHealth}");

        _slider.value = current / maxHealth;
    }
}