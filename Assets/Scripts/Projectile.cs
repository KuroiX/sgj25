using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    public float amplitude = 1f;        // size of sine wave
    public float frequency = 5f;        // speed of sine wave wobble

    [SerializeField] private Sprite flippedSprite;
    

    private bool _isFlipped;
    private SpriteRenderer _spriteRenderer;
    
    private float _timeAlive = 0f;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

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
        _timeAlive += Time.deltaTime;
        
        var direction = _isFlipped ? -2f : 1f;
        
        float sine = Mathf.Sin(_timeAlive * frequency) * amplitude;
        
        transform.Translate(Vector3.left * (Time.deltaTime * speed * direction) + Vector3.up * sine);
    }

    public void HitBoss()
    {
        if (_isFlipped) return;
        
        _isFlipped = true;
        _spriteRenderer.sprite = flippedSprite;

        StartCoroutine(WaitThenDie());
    }

    private IEnumerator WaitThenDie()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}