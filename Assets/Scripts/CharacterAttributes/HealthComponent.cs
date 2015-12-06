using UnityEngine;
using System.Collections;
using System;

public class HealthComponent : MonoBehaviour {

    private float m_health;

    public float StartingHealth = 100;
    public float MaxHealth = 100;
    public bool CanHaveNegativeHealth = false;
    public bool CanHaveMoreThanMaxHealth = false;

    public delegate void HealthChangedEventHandler(object sender, float _oldHealth, float _newHealth);
    public event HealthChangedEventHandler HealthChanged;

    void Start()
    {
        Health = StartingHealth;
    }

    public bool HasMaxHealth()
    {
        if (CanHaveMoreThanMaxHealth)
            return false;
        else
            return Health >= MaxHealth;
    }

    public float Health
    {
        get
        {
            return m_health;
        }

        set
        {
            float oldHealth = m_health;

            if (value > MaxHealth)
            {
                if (CanHaveMoreThanMaxHealth)
                    m_health = value;
                else
                    m_health = MaxHealth;
            }
            else if (value < 0)
            {
                if (CanHaveNegativeHealth)
                    m_health = value;
                else
                    m_health = 0;
            }
            else
            {
                m_health = value;
            }

            OnHealthChanged(oldHealth, m_health);
        }
    }

    protected void OnHealthChanged(float oldHealth, float newHealth)
    {
        if (HealthChanged != null)
        {
            HealthChanged(this, oldHealth, newHealth);
        }
    }
}
