using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float amplitude = 1f;        // size of sine wave
    [SerializeField] private float frequency = 5f;        // speed of sine wave wobble

    [SerializeField] private Sprite flippedSprite;

    [SerializeField] private bool verticalMovement;

    private bool _isFlipped;
    private SpriteRenderer _spriteRenderer;
    
    private float _timeAlive = 0f;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isFlipped)
        {
            Debug.Log(other.name);
            if (other.CompareTag("Boss"))
            {
                other.GetComponent<Health>().GetHit();
                Destroy(gameObject);
            }
        }
        else
        {
            if (other.CompareTag("Despawner"))
            {
                Destroy(gameObject);
            }

            if (!other.CompareTag("Player")) return;

            other.GetComponent<Health>().GetHit();

            Destroy(gameObject);
        }
    }

    private void Update()
    {
        _timeAlive += Time.deltaTime;

        if (!verticalMovement || _isFlipped)
        {
            var direction = _isFlipped ? -2f : 1f;
        
            float sine = _isFlipped ? 0 : Mathf.Sin(_timeAlive * frequency) * amplitude;
            
            transform.Translate(Vector3.left * (Time.deltaTime * speed * direction) + Vector3.up * sine);
        }
        else
        {
            transform.Translate( Vector3.down * (Time.deltaTime * speed));
        }
        
    }

    public void HitBoss()
    {
        if (_isFlipped) return;
        
        _isFlipped = true;
        _spriteRenderer.sprite = flippedSprite;

        //StartCoroutine(WaitThenDie());
    }

    private IEnumerator WaitThenDie()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}