using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class BodySegment : MonoBehaviour, IDamageable
{
    public PlayerController Player { get; set; }
    public int CurrentHealth { get { return Player.CurrentHealth; } set { } }

    private Rigidbody2D rBdoy;
    private SpriteRenderer spriteRenderer;
    private float segmentSize;
    private bool canDamage = true;

    [Header("Decouple Parameters")]
    [Range(0.1f, 1f)]
    [Tooltip("Gravity of this segment rigidbody when decoupled from the main body.")]
    [SerializeField] private float gravityScale = .3f;
    [Range(0f, 5f)]
    [Tooltip("Drag of this segment rigidbody when decoupled from the main body.")]
    [SerializeField] private float drag = 2f;
    [Tooltip("Force used to decouple a segment when it losses all his health.")]
    [SerializeField] private float decoupleForce = 300f;
    [Tooltip("Force used to rotate a segment that has been decoupled from the body.")]
    [SerializeField] private float torqueForce = 15f;
    [Tooltip("Time for a decoupled segment to despawn.")]
    [SerializeField] private float despawnTime = 20f;

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

    /// <summary>
    /// Decouples this segment from the main player body and adds force and rotation to it's rigidbody to simulate the separation. Destroys the segment at the specified time.
    /// </summary>
    public void Decouple()
    {
        rBdoy.AddRelativeForce(-transform.right * decoupleForce);
        rBdoy.AddTorque(torqueForce);
        rBdoy.gravityScale = gravityScale;
        rBdoy.drag = drag;
        canDamage = false;

        Destroy(gameObject, despawnTime);
    }

    public bool Damage(int amount)
    {
        if (canDamage)
            return Player.Damage(amount);
        return false;
    }

    public void Death()
    {
        Player.Death();
    }
}