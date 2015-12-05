﻿using UnityEngine;
using System.Collections;

public class PowerComponent : MonoBehaviour {

    private float m_power;

    public float StartingPower = 100;
    public float MaxPower = 100;
    public bool CanHaveNegativePower = false;
    public bool CanHaveMoreThanMaxPower = false;

    public delegate void PowerChangedEventHandler(object sender, float _oldPower, float _newPower);
    public event PowerChangedEventHandler PowerChanged;

    void Start()
    {
        Power = StartingPower;
    }

    public float Power
    {
        get
        {
            return m_power;
        }

        set
        {
            float oldPower = m_power;

            if (value > MaxPower)
            {
                if (CanHaveMoreThanMaxPower)
                    m_power = value;
                else
                    m_power = MaxPower;
            }
            else if (value < 0)
            {
                if (CanHaveNegativePower)
                    m_power = value;
                else
                    m_power = 0;
            }
            else
            {
                m_power = value;
            }

            OnPowerChanged(oldPower, m_power);
        }
    }

    public bool CanLaunchAttack(float cost)
    {
        return cost <= Power;
    }

    protected void OnPowerChanged(float oldPower, float newPower)
    {
        if (PowerChanged != null)
            PowerChanged(this, oldPower, newPower);
    }
}
