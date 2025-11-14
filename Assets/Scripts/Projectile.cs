using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Destroy(gameObject);
    }

    private void Update()
    {
        transform.Translate(Vector3.left * (Time.deltaTime * speed));
    }
}
