using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [Header("Target Options")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform mouse;
    [Range(2, 10)]
    [SerializeField] private float targetDivider = 4;
    [Header("Debug")]
    [SerializeField] private bool showDebug;

    private void FixedUpdate()
    {
        CalculatePosition();
    }

    /// <summary>
    /// Calculates the point between two transforms depending on the targetDivider variable and sets this GameObject transform to that point.
    /// </summary>
    private void CalculatePosition()
    {
        Vector3 newPosition = (mouse.position + (targetDivider - 1) * player.position) / targetDivider;
        transform.position = newPosition;
    }

    private void OnDrawGizmos()
    {
        if (!showDebug) return;

        Gizmos.DrawWireSphere(transform.position, 1f);
        Gizmos.DrawLine(player.position, mouse.position);
    }
}
