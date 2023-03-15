using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletLifeTime { get; set; } = 3;

    [SerializeField] private int bulletDamage = 5;
    [SerializeField] private bool isEnemyBullet = false;

    private void Awake()
    {
        Destroy(gameObject, BulletLifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isEnemyBullet) {
            if (collision.gameObject.tag == "Enemy" || collision.gameObject.layer == 13)
                return;

            if (collision.collider.TryGetComponent(out IDamageable damageable))
            {
                if (damageable.Damage(bulletDamage))
                    damageable.Death();
            }

            StartCoroutine(PopAnim());
        } else {
            if (collision.gameObject.tag == "Player" || collision.gameObject.layer == 3 || collision.gameObject.tag == "PlayerHead")
                return;

            if (collision.collider.TryGetComponent(out IDamageable damageable))
            {
                if (damageable.Damage(bulletDamage))
                    damageable.Death();
            }

            StartCoroutine(PopAnim());
        }
    }

    private IEnumerator PopAnim() {
        yield return new WaitForEndOfFrame();
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
        GetComponent<Animator>().SetTrigger("OnDestroy");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
