using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElectricField : InteractableObject
{
    private BoxCollider2D collisionArea;
    private List<IDamageable> collidingBodies = new List<IDamageable>();
    private bool isActive;

    [SerializeField] private int damage;
    [SerializeField] private float damageTick;
    [SerializeField] private LayerMask collisionLayers;
    [SerializeField] private ParticleSystem electricFieldEffect;

    private void Awake()
    {
        collisionArea = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        var shape = electricFieldEffect.shape;
        shape.scale = collisionArea.size;

        Invoke("Electrocute", damageTick);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if object that collides is within the layermask
        if (!((collisionLayers.value & (1 << collision.gameObject.layer)) > 0)) { return; }

        if(collision.TryGetComponent(out IDamageable damageable))
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

    public override void OnActivation()
    {
        collisionArea.enabled = true;
        electricFieldEffect.Play();
        isActive = true;
    }

    public override void OnDeactivation()
    {
        collisionArea.enabled = false;
        electricFieldEffect.Stop();
        isActive= false;
    }
}
