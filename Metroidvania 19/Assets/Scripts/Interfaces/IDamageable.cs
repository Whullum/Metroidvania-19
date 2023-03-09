public interface IDamageable
{
    /// <summary>
    /// Current health of this object.
    /// </summary>
    public int CurrentHealth { get; set; } // Use this for managing objects health

    /// <summary>
    /// Damages this object with the specified amount.
    /// </summary>
    /// <param name="amount">Amount of damage to deal to this object.</param>
    /// <returns>If the object has lost all his health.</returns>
    public bool Damage(int amount);

    /// <summary>
    /// When the object has lost all his health, this takes care of the despawn/reset logic.
    /// </summary>
    public void Death();
}
