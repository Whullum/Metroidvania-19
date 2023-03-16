using UnityEngine;
using UnityEngine.Tilemaps;

public class WeakWall : MonoBehaviour, IDamageable
{
    public int CurrentHealth { get { return health; } set { health = value; } }

    [SerializeField] private int health = 10;
    [SerializeField] private ParticleSystem wallDestroyedEffect;
    [SerializeField] private AK.Wwise.Event wallBreakingSound;

    [SerializeField]
    private Tilemap tilemap;

    private void Awake()
    {
        
    }

    private void Start()
    {
        health = (health == 0) ? 10 : health;

        tilemap = GetComponentInParent<Tilemap>();
    }

    public bool Damage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            return true;
        }


        return false;
    }

    public void Death()
    {
        Instantiate(wallDestroyedEffect, transform.position, Quaternion.identity);
        wallBreakingSound.Post(gameObject);
        
        Destroy(gameObject);
        tilemap.SetTile(tilemap.WorldToCell(this.gameObject.transform.position), null);
    }
}
