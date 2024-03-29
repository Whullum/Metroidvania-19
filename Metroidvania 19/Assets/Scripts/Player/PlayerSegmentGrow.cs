using UnityEngine;

public class PlayerSegmentGrow : MonoBehaviour
{
    [SerializeField] private ParticleSystem meatEatedEffect;

    private void Start() {
        LeanTween.moveLocalY(gameObject, transform.localPosition.y + 0.5f, 1f).setLoopPingPong().setEaseInOutQuad();
        meatEatedEffect = GetComponentInChildren<ParticleSystem>();
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
            ParticleSystem.MainModule main = meatEatedEffect.main;
            main.loop = false;

            Destroy(gameObject);
        }
    }
}
