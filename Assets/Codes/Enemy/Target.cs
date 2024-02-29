using UnityEngine;
using NaughtyAttributes;
using System;

public class Target : MonoBehaviour , IDamagable
{
    public TargetType targetType;
    [SerializeField] protected float health;

    [SerializeField] protected Vector3 startVelocity;


    public float BaseHealth { get; private set; }
    public float CurrentHealth { get; private set; }

    private void Start()
    {
        BaseHealth = health;
        CurrentHealth = BaseHealth;

        GetComponent<Rigidbody>().velocity = startVelocity;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if(CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
[Flags]
public enum TargetType
{
    Small = 1, 
    Medium = 2, 
    Large = 4
}