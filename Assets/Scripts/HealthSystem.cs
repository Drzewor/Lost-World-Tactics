using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{

    public event EventHandler OnDead;
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    [SerializeField] private int health;
    [SerializeField] private int healthMax = 100;

    private void Awake() 
    {
        health = healthMax;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if(health < 0)
        {
            health = 0;
        }

        OnDamaged.Invoke(this,EventArgs.Empty);

        if(health == 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        health += healAmount;

        if(health > healthMax)
        {
            health = healthMax;
        }

        OnHealed.Invoke(this,EventArgs.Empty);
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }
}
