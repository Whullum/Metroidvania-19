using UnityEngine;

public class PlayerSegmentGrow : MonoBehaviour
{
    [SerializeField] private ParticleSystem meatEatedEffect;

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
