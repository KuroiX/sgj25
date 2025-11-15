using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;

    private bool _isFlipped;

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
        var direction = _isFlipped ? -2f : 1f;
        
        transform.Translate(Vector3.left * (Time.deltaTime * speed * direction));
    }

    public void HitBoss()
    {
        if (_isFlipped) return;
        
        _isFlipped = true;

        StartCoroutine(WaitThenDie());
    }

    private IEnumerator WaitThenDie()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}