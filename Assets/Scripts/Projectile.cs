using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float amplitude = 1f;        // size of sine wave
    [SerializeField] private float frequency = 5f;        // speed of sine wave wobble

    [SerializeField] private Sprite flippedSprite;

    [SerializeField] private bool verticalMovement;

    [SerializeField] private Color playerColor;

    private bool _isFlipped;
    private SpriteRenderer _spriteRenderer;
    
    private float _timeAlive = 0f;

    [SerializeField] private ParticleSystem hitEffectInstance;
    [SerializeField] private float shakeStrength = 0.1f;
    

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isFlipped)
        {
            if (other.CompareTag("Boss"))
            {
                other.GetComponent<Health>().HitParry();
                other.transform.DOShakePosition(0.5f, shakeStrength);
                hitEffectInstance.startColor = playerColor;
                StartCoroutine(DestroyRoutine());
            }
        }
        else
        {
            if (other.CompareTag("Despawner"))
            {
                Destroy(gameObject);
            }

            if (!other.CompareTag("Player")) return;
            
            var player = other.GetComponent<Player>();
            
            if (player.IsInvincible) return;

            player.Stun();
            other.GetComponent<Health>().GetHit();

            StartCoroutine(DestroyRoutine());
        }
    }

    private IEnumerator DestroyRoutine()
    {
        hitEffectInstance.gameObject.SetActive(true);
        _spriteRenderer.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
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
    }
}