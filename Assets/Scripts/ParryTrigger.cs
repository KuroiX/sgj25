using System;
using System.Collections.Generic;
using UnityEngine;

public class ParryTrigger: MonoBehaviour
{
    private readonly List<Collider2D> _enteredColliders = new List<Collider2D>();

    public event Action<bool> OnEntered;

    public bool HasHit { get; private set; }

    [SerializeField] private Player player;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Parry") && (!other.CompareTag("SecondParry") || !player.IsInAggroMode)) return;
        
        OnEntered?.Invoke(true);
        
        _enteredColliders.Add(other);
        HasHit = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Parry") && (!other.CompareTag("SecondParry"))) return;
        
        if (_enteredColliders.Contains(other)) _enteredColliders.Remove(other);
        
        if (_enteredColliders.Count != 0) return;
        
        HasHit = false;
    }

    public void DoParry()
    {
        for (int i = _enteredColliders.Count - 1; i >= 0; i--)
        {
            var col = _enteredColliders[i];
            col.GetComponent<Projectile>().HitBoss();
            //_enteredColliders.Remove(col);
            //if (col.gameObject) Destroy(col.gameObject);
        }
    }
}