using System.Collections;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    private LayerMask playerLayer;
    private bool isAttacking;
    private AbilitiesShader shader;

    [SerializeField] AK.Wwise.Event biteSound;

    [Header("Attack Properties")]
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackRadius = .7f;
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform attackPosition;
    [Header("Debug Info")]
    [SerializeField] private bool debugInfo;

    private void Awake() {
        playerLayer = LayerMask.GetMask("Player", "PlayerSegment");
        shader = FindObjectOfType<AbilitiesShader>();
    }

    private void Update() => GetInput();
    
    private void GetInput()
    {
        if(Input.GetKeyDown(KeyCode.Q) && !isAttacking) {
            StartCoroutine(shader.MeleeCooldown((int)attackCooldown));
            StartCoroutine(Attack());
        }
    }

    private void OnEnable() {
        if (shader.enabled) { StartCoroutine(shader.toggleAnim(0, true)); }
    }

    private void OnDisable() {
        if (shader.enabled) { StartCoroutine(shader.toggleAnim(0, false)); }
    }
        
    private IEnumerator Attack()
    {
        yield return new WaitForEndOfFrame();
        isAttacking = true;

        // ** MELEE ATTACK SOUND ** 
        GetComponent<PlayerSounds>().PlayBiteSound();
        GetComponent<PlayerAnimation>().TriggerBiteAnimaton();

        Collider2D[] damagedEntities = Physics2D.OverlapCircleAll(attackPosition.position, attackRadius, ~playerLayer);

        if (damagedEntities != null && damagedEntities.Length > 0)
        {
            for (int i = 0; i < damagedEntities.Length; i++)
            {
                if (damagedEntities[i] != null && damagedEntities[i].TryGetComponent(out IDamageable damageable))
                    if (damageable.Damage(attackDamage))
                        damageable.Death();
            }
        }

        yield return new WaitForSeconds(attackCooldown);

        GetComponent<PlayerAnimation>().ResetTriggerBiteAnimation();
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        if (!debugInfo) return;

        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }
}
