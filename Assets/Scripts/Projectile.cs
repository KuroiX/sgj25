using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Despawner"))
        {
            Destroy(gameObject);
        }
        
        if (!other.CompareTag("Player")) return;

        other.GetComponent<Health>().GetHit();

        Destroy(gameObject);
    }

    private void Update()
    {
        transform.Translate(Vector3.left * (Time.deltaTime * speed));
    }
}
