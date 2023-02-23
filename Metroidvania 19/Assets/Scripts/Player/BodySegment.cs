using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BodySegment : MonoBehaviour
{
    private Rigidbody2D rBdoy;
    private SpriteRenderer spriteRenderer;
    private float segmentSize;

    private void Awake()
    {
        rBdoy = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        segmentSize = GetComponent<Collider2D>().bounds.size.x;
    }

    public void Move(Vector3 prevPosition, float segmentSeparation, Quaternion newRotation)
    {
        // Calculate the next move point with this segment size and segment separation
        Vector3 moveDirection = (prevPosition - transform.position).normalized;
        Vector3 newPosition = prevPosition - moveDirection * (segmentSize + segmentSeparation);

        // Move body accordingly
        rBdoy.MovePosition(newPosition);
        rBdoy.MoveRotation(newRotation);

        // Flip sprite renderer depending on where this GameObject is facing
        if (transform.right.x >= 0)
            spriteRenderer.flipY = false;
        else
            spriteRenderer.flipY = true;
    }

    /// <summary>
    /// Sets the order in layer for this segment.
    /// </summary>
    /// <param name="sortingOrder">Order in layer for this segment.</param>
    public void SetSortingOrder(int sortingOrder) => spriteRenderer.sortingOrder = sortingOrder;
}