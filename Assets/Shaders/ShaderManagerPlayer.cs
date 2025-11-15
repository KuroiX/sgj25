using System;
using UnityEngine;

public class ShaderManagerPlayer : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject rageVisuals;

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
        rageVisuals.SetActive(false);
    }

    private void PlayerOnAggroChanged(bool aggroEnabled)
    {
        rageVisuals.SetActive(aggroEnabled);
    }
}
