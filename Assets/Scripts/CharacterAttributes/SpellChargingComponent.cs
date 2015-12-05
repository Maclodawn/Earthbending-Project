using UnityEngine;
using System.Collections;

public class SpellChargingComponent : MonoBehaviour {

    enum State
    {
        READY,
        COOLDOWN,
        CHARGING,
        IDLE,
    }

    public delegate void ReadinessChangedEventHandler(object sender, float _oldReadiness, float _newReadiness);
    public event ReadinessChangedEventHandler ReadinessChanged;

    private State m_state;
    private float m_readiness;

    public float ChargeRate;
    public float CooldownRate;
    public bool ResetWhenChargeStop = true;

    public object Tag;

    public float Readiness
    {
        get { return m_readiness; }
        set
        {
            float oldValue = m_readiness;
            m_readiness = value;
            OnReadinessChanged(oldValue, m_readiness);
        }
    }

    public void StartCharging()
    {
        if(m_state == State.IDLE)
        {
            if (ChargeRate > 0)
                m_state = State.CHARGING;
            else
            {
                m_state = State.READY;
                Readiness = 1;
            }
        }
    }

    public void StopCharging()
    {
        if(m_state == State.CHARGING)
        {
            if(ResetWhenChargeStop)
                Readiness = 0;
            
            m_state = State.IDLE;
        }
    }

    public bool ReadyToCharge()
    {
        return m_state == State.IDLE;
    }

    public bool Ready()
    {
        return m_state == State.READY;
    }

    public void LaunchSpell()
    {
        if(Ready())
        {
            if(CooldownRate > 0)
                m_state = State.COOLDOWN;
            else
            {
                m_state = State.IDLE;
                Readiness = 0;
            }
        }

        StopCharging();
    }

	// Use this for initialization
	void Start () {
        Readiness = 0;
        m_state = State.IDLE;
	}
	
	// Update is called once per frame
	void Update () {
	    switch(m_state)
        {
            case State.CHARGING:
                Readiness += ChargeRate * Time.deltaTime;
                break;

            case State.COOLDOWN:
                Readiness -= CooldownRate * Time.deltaTime;
                break;

            case State.IDLE:
            case State.READY:
            default:
                break;
        }

        UpdateState();
	}

    void UpdateState()
    {
        if (Readiness <= 0 && m_state != State.IDLE)
        {
            Readiness = 0;
            m_state = State.IDLE;
        }

        if (Readiness >= 1 && m_state != State.READY)
        {
            Readiness = 1;
            m_state = State.READY;
        }
    }

    protected void OnReadinessChanged(float oldReadiness, float newReadiness)
    {
        if (ReadinessChanged != null)
            ReadinessChanged(this, oldReadiness, newReadiness);
    }
}
