using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricField : InteractableObject
{
    private BoxCollider2D collisionArea;
    private List<IDamageable> collidingBodies = new List<IDamageable>();
    private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    private bool isActive;

    [SerializeField] private int damage;
    [SerializeField] private float damageTick;
    [SerializeField] private LayerMask collisionLayers;
    [SerializeField] private ParticleSystem[] effects;
    [Header("Electric Field Sprites")]
    [SerializeField] private SpriteRenderer topRight;
    [SerializeField] private SpriteRenderer bottomRight;
    [SerializeField] private SpriteRenderer bottomLeft;
    [SerializeField] private SpriteRenderer topLeft;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;

    private void Awake()
    {
        collisionArea = GetComponent<BoxCollider2D>();
        SetSpritePositions();
    }

    private void Start()
    {
        Invoke("Electrocute", damageTick);
        OnActivation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if object that collides is within the layermask
        if (!((collisionLayers.value & (1 << collision.gameObject.layer)) > 0)) { return; }

        if (collision.TryGetComponent(out IDamageable damageable))
            collidingBodies.Add(damageable);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if object that collides is within the layermask
        if (!((collisionLayers.value & (1 << collision.gameObject.layer)) > 0)) { return; }

        if (collision.TryGetComponent(out IDamageable damageable))
            collidingBodies.Remove(damageable);
    }

    private void Electrocute()
    {
        if (!isActive)
        {
            Invoke("Electrocute", damageTick);
            return;
        }

        foreach (IDamageable damageable in collidingBodies)
            if (damageable.Damage(damage))
                damageable.Death();

        Invoke("Electrocute", damageTick);
    }

    private void SetSpritePositions()
    {
        topRight.transform.position = collisionArea.bounds.max;
        bottomRight.transform.position = new Vector3(collisionArea.bounds.max.x, collisionArea.bounds.min.y);
        topLeft.transform.position = new Vector3(collisionArea.bounds.min.x, collisionArea.bounds.max.y);
        bottomLeft.transform.position = collisionArea.bounds.min;

        sprites.Add(topRight);
        sprites.Add(bottomRight);
        sprites.Add(topLeft);
        sprites.Add(bottomLeft);
    }

    public override void OnActivation()
    {
        collisionArea.enabled = true;
        isActive = true;

        foreach (SpriteRenderer sprite in sprites)
            sprite.sprite = activeSprite;

        for (int i = 0; i < effects.Length; i++)
            effects[i].Play();

        activationSound.Post(gameObject);
    }

    public override void OnDeactivation()
    {
        collisionArea.enabled = false;
        isActive = false;

        foreach (SpriteRenderer sprite in sprites)
            sprite.sprite = inactiveSprite;

        for (int i = 0; i < effects.Length; i++)
            effects[i].Stop();

        activationSound.Post(gameObject);
    }
}
