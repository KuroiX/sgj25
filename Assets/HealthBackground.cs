using UnityEngine;

public class HealthBackground : MonoBehaviour
{
    [SerializeField] private Health playerHealth;

    private bool _startHealthSet;

    private float _startHealth;

    private void OnEnable()
    {
        playerHealth.OnHealthChange += OnHealthChange;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChange -= OnHealthChange;
    }

    private void OnHealthChange(float currentHealth, float maxHealth)
    {
        if (!_startHealthSet) 
        {
            _startHealth = maxHealth;
            _startHealthSet = true;
        }
        
        var rectTransform = (RectTransform)transform;

        var result = (maxHealth / _startHealth) * 1000;
        rectTransform.sizeDelta = new Vector2(result + 20, rectTransform.sizeDelta.y);
        rectTransform.anchoredPosition = new Vector2(result / 2, rectTransform.anchoredPosition.y);
    }
}
