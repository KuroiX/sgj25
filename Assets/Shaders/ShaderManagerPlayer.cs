using System;
using UnityEngine;

public class ShaderManagerPlayer : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private SpriteRenderer rageVisuals;

    void OnEnable()
    {
        player.AggroChanged += PlayerOnAggroChanged;
    }

    void OnDisable()
    {
        player.AggroChanged -= PlayerOnAggroChanged;
    }

    private void Start()
    {
        rageVisuals.enabled = false;
    }

    private void PlayerOnAggroChanged(bool aggroEnabled)
    {
        rageVisuals.enabled = aggroEnabled;
    }
}
