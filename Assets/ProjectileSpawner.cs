using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private float randomRange = 5f;
    

    private float _timer;
    

    // Update is called once per frame
    private void Update()
    {
        _timer -= Time.deltaTime;
        
        if (_timer > 0f) return;
        
        Instantiate(projectilePrefab, transform.position + Vector3.up * Random.Range(0, randomRange), transform.rotation);

        _timer = Random.Range(0.5f, 1f);
    }
}
