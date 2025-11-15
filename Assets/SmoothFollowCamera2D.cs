using UnityEngine;

public class SmoothFollowCamera2D : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Follow Settings")]
    public float smoothTime = 0.2f;  // higher = slower, smoother
    private Vector3 _velocity = Vector3.zero;

    [Header("Bounds (World Space)")]
    public bool useBounds = false;
    public Vector2 minBounds;   // bottom-left corner
    public Vector2 maxBounds;   // top-right corner

    [Header("Z Offset")]
    public float zOffset = -10f;  // typical 2D camera offset

    void LateUpdate()
    {
        if (target is null) return;

        // Desired position (follow the target)
        Vector3 targetPos = new Vector3(target.position.x + 3, target.position.y, zOffset);

        // Smoothly move towards the target position
        Vector3 smoothPos = Vector3.SmoothDamp(
            transform.position,
            targetPos,
            ref _velocity,
            smoothTime
        );

        // If bounds are enabled, clamp the position
        if (useBounds)
        {
            smoothPos.x = Mathf.Clamp(smoothPos.x, minBounds.x, maxBounds.x);
            smoothPos.y = Mathf.Clamp(smoothPos.y, minBounds.y, maxBounds.y);
        }

        transform.position = smoothPos;
    }
}