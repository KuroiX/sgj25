using System;
using UnityEngine;

public class ParryManager : MonoBehaviour
{
    [SerializeField] private ParryTrigger inner;
    [SerializeField] private ParryTrigger middle;
    [SerializeField] private ParryTrigger outer;
    [SerializeField] private Player player;
    

    public ParryState TriggerParry()
    {
        if (inner.HasHit)
        {
            return ParryState.Late;
        }

        if (middle.HasHit)
        {
            return ParryState.Perfect;
        }

        if (outer.HasHit)
        {
            return ParryState.Early;
        }

        return ParryState.Miss;
    }

    private void OnEnable()
    {
        middle.OnEntered += MiddleOnOnEntered;
    }

    private void OnDisable()
    {
        middle.OnEntered -= MiddleOnOnEntered;
    }

    private void MiddleOnOnEntered(bool obj)
    {
        player.TriggerParry();
    }

    public void DoParry()
    {
        outer.DoParry();
        middle.DoParry();
        inner.DoParry();
    }
}