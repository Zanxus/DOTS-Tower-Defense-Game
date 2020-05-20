using System;
public class HealthSystem
{
    public event EventHandler OnHealthChanged;
    public event EventHandler OnMaxHealthChanged;
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDead;

    int health;
    int maxHealth;


    public HealthSystem(int maxHealth)
    {
        this.maxHealth = maxHealth;
        health = maxHealth;
    }
    public int GetHealth()
    {
        return health;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealthNormalized()
    {
        return (float)health / maxHealth;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;
        if (health < 0)
        {
            health = 0;
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnDamaged?.Invoke(this, EventArgs.Empty);
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnHealed?.Invoke(this, EventArgs.Empty);
    }
    public void HealFull()
    {
        health = maxHealth;
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public bool IsDead()
    {
        return health <= 0;
    }

    public void SetHealthMax(int healthMax, bool fullHealth)
    {
        this.maxHealth = healthMax;
        if (fullHealth) health = healthMax;
        OnMaxHealthChanged?.Invoke(this, EventArgs.Empty);
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }


}
