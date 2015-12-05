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

    private State m_state;
    private float m_readyness;

    public float ChargeRate;
    public float CooldownRate;
    public bool ResetWhenChargeStop;

    public object Tag;

    public void StartCharging()
    {
        if(m_state == State.IDLE)
            m_state = State.CHARGING;
    }

    public void StopCharging()
    {
        if(m_state == State.CHARGING)
        {
            if(ResetWhenChargeStop)
                m_readyness = 0;
            
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
            m_state = State.COOLDOWN;
        }

        StopCharging();
    }

	// Use this for initialization
	void Start () {
        m_readyness = 0;
        m_state = State.IDLE;
	}
	
	// Update is called once per frame
	void Update () {
	    switch(m_state)
        {
            case State.CHARGING:
                m_readyness += ChargeRate * Time.deltaTime;
                break;

            case State.COOLDOWN:
                m_readyness -= CooldownRate * Time.deltaTime;
                break;

            case State.IDLE:
            case State.READY:
            default:
                break;
        }

        UpdateState();

        // DEBUGGING
        if (Input.GetKeyDown(KeyCode.T))
            StartCharging();
        else if (Input.GetKeyUp(KeyCode.T))
            LaunchSpell();
	}

    void UpdateState()
    {
        if(m_readyness <= 0 && m_state != State.IDLE)
        {
            m_readyness = 0;
            m_state = State.IDLE;
        }

        if(m_readyness >= 1 && m_state != State.READY)
        {
            m_readyness = 1;
            m_state = State.READY;
        }
    }
}
