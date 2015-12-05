using UnityEngine;
using System.Collections;

public class PowerBarController : MonoBehaviour {

    public PowerComponent m_power;

    private UnityEngine.UI.Image m_bar;

    public void Setup(PowerComponent _power)
    {
        if (m_power != null)
            m_power.PowerChanged -= OnPowerChanged;

        m_power = _power;
        m_bar = GetComponent<UnityEngine.UI.Image>();

        if (m_bar != null && m_power != null)
        {
            m_power.PowerChanged += OnPowerChanged;
        }
    }

    // Use this for initialization
    void Start()
    {
        if (m_power != null)
            Setup(m_power);
    }

    private void OnPowerChanged(object sender, float oldPower, float newPower)
    {
        if (sender == m_power)
        {
            m_bar.rectTransform.localScale = new Vector3(newPower / m_power.MaxPower, 1, 1);
        }
    }
}
