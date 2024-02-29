using UnityEngine;
public interface IDamagable 
{

    public void TakeDamage(int damage);
    public void Die();
    public float BaseHealth { get; }
    public float CurrentHealth { get; }

}
