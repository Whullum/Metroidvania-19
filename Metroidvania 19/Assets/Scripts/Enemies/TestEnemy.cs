using UnityEngine;

public class TestEnemy : MonoBehaviour, IDamageable
{
    public int CurrentHealth { get { return currentHealth; } set { currentHealth = value; } }
    [SerializeField] private int currentHealth;

    public bool Damage(int amount)
    {
        if (currentHealth - amount <= 0)
            return true;

        currentHealth -= amount;
        return false;
    }

    public void Death()
    {
        GetComponent<Dropper>().Drop(true);
    }
}
