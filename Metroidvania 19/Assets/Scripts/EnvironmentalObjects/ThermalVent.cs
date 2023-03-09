using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThermalVent : MonoBehaviour
{
    private Collider2D collisionArea;
    private List<Rigidbody2D> collisionBodies = new List<Rigidbody2D>();
    private bool isActive;

    [SerializeField] private float activeTime;
    [SerializeField] private float cooldownTime;
    [SerializeField] private float pushForce;
    [SerializeField] private LayerMask collisionLayers;
    [SerializeField] private ParticleSystem ventEffect;

    private void Awake()
    {
        collisionArea= GetComponent<Collider2D>();
    }

    private void Start()
    {
        StartCoroutine(ActivateVent());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if object that collides is within the layermask
        if (!((collisionLayers.value & (1 << collision.gameObject.layer)) > 0)) { return; }

        if (collision.TryGetComponent(out Rigidbody2D collisionBody))
            collisionBodies.Add(collisionBody);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if object that collides is within the layermask
        if (!((collisionLayers.value & (1 << collision.gameObject.layer)) > 0)) { return; }

        if (collision.TryGetComponent(out Rigidbody2D collisionBody))
            if (collisionBodies.Contains(collisionBody))
                collisionBodies.Remove(collisionBody);
    }

    private void FixedUpdate()
    {
        if (isActive)
            PushObjects();
    }

    private void PushObjects()
    {
        foreach (Rigidbody2D rBody in collisionBodies)
        {
            rBody.AddForce(transform.up * pushForce, ForceMode2D.Force);
        }
    }

    private IEnumerator ActivateVent()
    {
        collisionArea.enabled = true;
        isActive = true;

        ventEffect.Play();

        yield return new WaitForSeconds(activeTime);

        collisionArea.enabled = false;
        isActive = false;

        ventEffect.Stop();

        yield return new WaitForSeconds(cooldownTime);

        StartCoroutine(ActivateVent());
    }
}
