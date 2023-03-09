using UnityEngine;

public class PlayerSegmentGrow : MonoBehaviour
{
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
                player.GrowBody();

            meatEatedEffect.Play();
            meatEatedEffect.transform.parent = null;
            meatEatedEffect.transform.localScale = Vector3.one;

            Destroy(gameObject);
        }
    }
}
