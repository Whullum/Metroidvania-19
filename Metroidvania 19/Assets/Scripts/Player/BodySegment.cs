using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BodySegment : MonoBehaviour
{
    private Rigidbody2D rBdoy;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rBdoy = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Move(Vector3 newPosition, Quaternion newRotation)
    {
        rBdoy.MovePosition(newPosition);
        rBdoy.MoveRotation(newRotation);

        if (transform.right.x >= 0)
            spriteRenderer.flipY = false;
        else
            spriteRenderer.flipY = true;
    }

    public void SetSortingOrder(int sortingOrder) => spriteRenderer.sortingOrder = sortingOrder;
}