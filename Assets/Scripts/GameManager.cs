using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Health bossHealth;
    
    private void OnEnable()
    {
        playerHealth.OnHealthChange += CheckLose;
        bossHealth.OnHealthChange += CheckWin;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChange -= CheckLose;
        bossHealth.OnHealthChange -= CheckWin;
    }

    private void CheckWin(float currentHealth, float maxHealth)
    {
        if (currentHealth > 0) return;
        
        Debug.Log("Win!");
    }

    private void CheckLose(float currentHealth, float maxHealth)
    {
        if (currentHealth > 0) return;
        
        Debug.Log("Lose!");
    }

}
