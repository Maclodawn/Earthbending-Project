using UnityEngine;
using System.Collections;

public class PowerKit : MonoBehaviour {

    private bool m_disabled = false;
    private float m_timer = 0;

    [Header("Power")]
    public float RestoreQuantity;
    public float Cooldown = 5;

    // Update is called once per frame
    void Update()
    {
        if (m_disabled)
        {
            m_timer += Time.deltaTime;
            if (m_timer >= Cooldown)
                Enable();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (m_disabled)
            return;

        PowerComponent power = collider.gameObject.GetComponent<PowerComponent>();
        if (power != null)
        {
            if(!power.HasMaxPower())
            {
                power.Power += RestoreQuantity;
                Disable();
            }
        }
    }

    void Disable()
    {
        Renderer renderer = GetComponent<Renderer>();
        Collider collider = GetComponent<Collider>();

        if (renderer != null)
            renderer.enabled = false;

        if (collider != null)
            collider.enabled = false;

        m_timer = 0;
        m_disabled = true;
    }

    void Enable()
    {
        Renderer renderer = GetComponent<Renderer>();
        Collider collider = GetComponent<Collider>();

        if (renderer != null)
            renderer.enabled = true;

        if (collider != null)
            collider.enabled = true;

        m_timer = 0;
        m_disabled = false;
    }
}
