public interface IDamageable
{
    public int CurrentHealth { get; set; }

    public bool Damage(int amount);

    public void Death();
}
