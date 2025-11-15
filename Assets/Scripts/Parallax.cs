using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float strength = 0.5f; // Parallax intensity
    private Transform cam;
    private Vector3 startPos;

    void Start()
    {
        cam = Camera.main.transform;
        startPos = transform.position;
    }

    void Update()
    {
        Vector3 camDelta = cam.position;
        transform.position = new Vector3(
            startPos.x + camDelta.x * strength,
            startPos.y + camDelta.y * strength,
            startPos.z
        );
    }
}