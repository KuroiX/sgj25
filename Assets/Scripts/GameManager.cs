using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Health bossHealth;

    [SerializeField] private GameObject chillOut;
    [SerializeField] private GameObject burnOut;

    private bool _gameOver;
    
    
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
        if (currentHealth > 0 || _gameOver) return;
        
        chillOut.SetActive(true);
        _gameOver = true;
        StartCoroutine(EndGameRoutine());
    }

    private void CheckLose(float currentHealth, float maxHealth)
    {
        if (maxHealth > 0 || _gameOver) return;
        
        burnOut.SetActive(true);
        _gameOver = true;
        StartCoroutine(EndGameRoutine());
    }

    private IEnumerator EndGameRoutine()
    {
        yield return new WaitForSeconds(3);
        FindFirstObjectByType<SceneLoader>().LoadSceneByIndex(0);
    }

}
