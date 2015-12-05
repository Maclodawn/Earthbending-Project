using UnityEngine;
using System.Collections;

public class HealthBarController : MonoBehaviour {

    public HealthComponent m_life;

    private UnityEngine.UI.Image m_bar;

    public void Setup(HealthComponent _life)
    {
        if (m_life != null)
            m_life.HealthChanged -= OnHealthChanged;

        m_life = _life;
        m_bar = GetComponent<UnityEngine.UI.Image>();

        if (m_bar != null && m_life != null)
        {
            m_life.HealthChanged += OnHealthChanged;
        }
    }

	// Use this for initialization
	void Start () {
        if (m_life != null)
            Setup(m_life);
	}

    private void OnHealthChanged(object sender, float oldHealth, float newHealth)
    {
        if(sender == m_life)
        {
            m_bar.rectTransform.localScale = new Vector3(Mathf.Lerp(0, 1, newHealth / m_life.MaxHealth), 1, 1);
        }
    }
}
