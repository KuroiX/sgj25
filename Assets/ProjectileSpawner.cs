using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;

    [SerializeField] private float randomRange = 5f;
    [SerializeField] private float frequencyLower = 0.5f;
    [SerializeField] private float frequencyUpper = 1f;

    [SerializeField] private bool horizontal;

    private float _timer;

    // Update is called once per frame
    private void Update()
    {
        _timer -= Time.deltaTime;
        
        if (_timer > 0f) return;

        var direction = horizontal ? Vector3.right : Vector3.up;
        
        Instantiate(projectilePrefab, transform.position + direction * Random.Range(0, randomRange), transform.rotation);

        _timer = Random.Range(frequencyLower, frequencyUpper);
    }
}
