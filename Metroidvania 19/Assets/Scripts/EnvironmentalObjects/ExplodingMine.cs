using System.Collections;
using UnityEngine;

public class ExplodingMine : MonoBehaviour
{
    private Vector3 startPos;

    [Header("Mine Properties")]
    [Tooltip("Amount of damage this mine deals to all GameObjects inside it's radius.")]
    [SerializeField] private int damage = 1;
    [Tooltip("When something collides with the mine, the countdown time to explosion. Can be 0 to explode on contact.")]
    [SerializeField] private float activationTime = 1f;
    [Tooltip("Radius of the explosion.")]
    [SerializeField] private float explosionRadius = 5f;
    [Tooltip("Force wich all the rigidbodies inside the mine radius willl be pushed.")]
    [SerializeField] private float explosionForce = 25f;
    [Tooltip("Layers that can activaye this mine on collision.")]
    [SerializeField] private LayerMask collisionLayers;
    [Tooltip("Explosion effect.")]
    [SerializeField] private ParticleSystem explosionEffect;
    [Header("Movement Properties")]
    [Tooltip("Vertical movement scale.")]
    [Range(0, 5)]
    [SerializeField] private float heightScale;
    [Tooltip("Speed of the movement.")]
    [Range(0, 5)]
    [SerializeField] private float speed;
    [Header("Debug")]
    [SerializeField] private bool debugInfo;

    private void Start()
    {
        startPos = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if object that collides is within the layermask
        if (!((collisionLayers.value & (1 << collision.gameObject.layer)) > 0)) { return; }

        StartCoroutine(Explode());
    }

    private void Update()
    {
        Motion();
    }

    /// <summary>
    /// Moves the mine in a sine wave curve with noise.
    /// </summary>
    private void Motion()
    {
        float height = heightScale * Mathf.PerlinNoise(Time.time * speed, 0f) + startPos.y;
        Vector3 newPos = transform.position;
        newPos.y = height;
        transform.position = newPos;
    }

    private IEnumerator Explode()
    {
        // Wait for the activation time
        yield return new WaitForSeconds(activationTime);

        // Check all gameObjects inside attack radius
        Collider2D[] entities = Physics2D.OverlapCircleAll(transform.position, explosionRadius, collisionLayers);

        // Check if something was hit by the mine
        if (entities.Length > 0)
        {
            for (int i = 0; i < entities.Length; i++)
            {
                // Check if can be damage
                if (entities[i].TryGetComponent(out IDamageable damageable))
                    if (damageable.Damage(damage))
                        damageable.Death();

                // Check if can be pushed
                if (entities[i].TryGetComponent(out Rigidbody2D rBdoy))
                {
                    Vector2 explosionDirection = entities[i].transform.position - transform.position;
                    rBdoy.AddForce(explosionDirection.normalized * explosionForce, ForceMode2D.Impulse);
                }
            }
        }
        // Shake camera
        CameraController.ShakeCamera?.Invoke();
        // Create particle effect
        Instantiate(explosionEffect, transform.position, explosionEffect.transform.rotation);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (!debugInfo) return;

        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
