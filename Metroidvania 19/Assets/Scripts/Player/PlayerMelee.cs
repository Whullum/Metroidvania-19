using System.Collections;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    private LayerMask playerLayer;
    private bool isAttacking;

    [Header("Attack Properties")]
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackRadius = .7f;
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform attackPosition;
    [Header("Debug Info")]
    [SerializeField] private bool debugInfo;

    private void Awake() => playerLayer = LayerMask.GetMask("Player", "PlayerSegment");

    private void Update() => GetInput();

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isAttacking)
            StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        isAttacking = true;

        // ** MELEE ATTACK SOUND ** 

        Collider2D[] damagedEntities = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius, ~playerLayer);

        if (damagedEntities.Length > 0)
        {
            for (int i = 0; i < damagedEntities.Length; i++)
            {
                if (damagedEntities[i].TryGetComponent(out IDamageable damageable))
                    if (damageable.Damage(attackDamage))
                        damageable.Death();
            }
        }

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        if (!debugInfo) return;

        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }
}
