using UnityEngine;

public class ParryManager : MonoBehaviour
{
    [SerializeField] private ParryTrigger inner;
    [SerializeField] private ParryTrigger middle;
    [SerializeField] private ParryTrigger outer;

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
}