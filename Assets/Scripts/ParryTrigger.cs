using System.Collections.Generic;
using UnityEngine;

public class ParryTrigger: MonoBehaviour
{
    private readonly List<Collider2D> _enteredColliders = new List<Collider2D>();

    public bool HasHit { get; private set; }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Parry")) return;
        
        Debug.Log(other.name);
        
        _enteredColliders.Add(other);
        HasHit = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Parry")) return;

        _enteredColliders.Remove(other);
        
        if (_enteredColliders.Count != 0) return;
        
        HasHit = false;
    }
}