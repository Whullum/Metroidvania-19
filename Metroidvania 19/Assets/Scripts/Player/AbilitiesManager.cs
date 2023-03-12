using UnityEngine;

[RequireComponent(typeof(PlayerMelee))]
[RequireComponent(typeof(PlayerProjectile))]
[RequireComponent(typeof(PlayerDash))]
public class AbilitiesManager : MonoBehaviour
{
    private PlayerMelee melee;
    private PlayerProjectile projectile;
    private PlayerDash dash;

    [SerializeField] private bool Melee = false;
    [SerializeField] private bool Projectile = false;
    [SerializeField] private bool Dash = false;

    public bool MeleeAbility { get => Melee; set => Melee = value; }
    public bool ProjectileAbility { get => Projectile; set => Projectile = value; }
    public bool DashAbility { get => Dash; set => Dash = value; }

    private void Awake()
    {
        melee = GetComponent<PlayerMelee>();
        projectile = GetComponent<PlayerProjectile>();
        dash = GetComponent<PlayerDash>();
    }

    void Update()
    {
        melee.enabled = Melee;
        projectile.enabled = Projectile;
        dash.enabled = Dash;
    }
}
