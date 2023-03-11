using UnityEngine;

public class Spikes : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void Start()
    {
        damage = (damage == 0) ? 1 : damage;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out IDamageable damageable))
            if (damageable.Damage(damage))
                damageable.Death();
    }
}
