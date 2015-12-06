using UnityEngine;
using System.Collections;

public class ChargeBarController : MonoBehaviour {

    public SpellChargingComponent m_spell;

    private UnityEngine.UI.Image m_bar;

    public void Setup(SpellChargingComponent _spell)
    {
        if (m_spell != null)
            m_spell.ReadinessChanged -= OnReadinessChanged;

        m_spell = _spell;
        m_bar = GetComponent<UnityEngine.UI.Image>();

        if (m_bar != null && m_spell != null)
        {
            m_spell.ReadinessChanged += OnReadinessChanged;
        }
    }

	// Use this for initialization
	void Start () {
        if (m_spell != null)
            Setup(m_spell);
	}

    private void OnReadinessChanged(object sender, float oldReadiness, float newReadiness)
    {
        if (sender == m_spell)
        {
            m_bar.rectTransform.localScale = new Vector3(1, newReadiness, 1);
        }
    }
}
