using UnityEngine;

public class PlayerHealthRestore : DroppedObject
{
    [Range(1, 100)]
    [SerializeField] private int restoreAmount = 1;
    [SerializeField] private float despawnTime = 15f;
    [SerializeField] private ParticleSystem meatEatedEffect;

    private void Start()
    {
        Destroy(gameObject, despawnTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out PlayerController player))
                player.RestoreHealth(restoreAmount);

            meatEatedEffect.Play();
            meatEatedEffect.transform.parent = null;
            meatEatedEffect.transform.localScale = Vector3.one;

            Destroy(gameObject);
        }
    }
}
