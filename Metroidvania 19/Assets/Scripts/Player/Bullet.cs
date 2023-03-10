using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletLifeTime { get; set; } = 3;

    [SerializeField] private int bulletDamage = 5;

    private void Awake()
    {
        Destroy(gameObject, BulletLifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.layer == 3 || collision.gameObject.tag == "PlayerHead")
            return;

        if (collision.collider.TryGetComponent(out IDamageable damageable))
        {
            if (damageable.Damage(bulletDamage))
                damageable.Death();
        }

        Destroy(gameObject);
    }
}
