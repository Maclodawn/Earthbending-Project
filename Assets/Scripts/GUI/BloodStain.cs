using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BloodStain : MonoBehaviour {

    private LinkedList<UnityEngine.UI.Image> m_Shown = new LinkedList<UnityEngine.UI.Image>();
    private HealthComponent health;
    private LinkedList<UnityEngine.UI.Image> m_BloodStainBank = new LinkedList<UnityEngine.UI.Image>();

    public List<UnityEngine.UI.Image> BloodStainBank = new List<UnityEngine.UI.Image>();
    public float HideRate = 0.5f;
    public float HeavyDamageIndicator = 25;

    void Start()
    {
        foreach (var stain in BloodStainBank)
            m_BloodStainBank.AddLast(stain);
    }

    public void Setup(HealthComponent h)
    {
        if (health != null)
            health.HealthChanged -= OnHealthChanged;

        health = h;
        if (health != null)
            health.HealthChanged += OnHealthChanged;
    }

    void Update()
    {
        foreach(var stain in m_Shown)
        {
            var alpha = stain.color.a;
            alpha -= HideRate * Time.deltaTime;
            if (alpha < 0)
            {
                stain.color = new Color(1, 1, 1, 0);
                GiveBackStain(stain);
            }
            else
            {
                stain.color = new Color(1, 1, 1, alpha);
            }
        }
    }

    UnityEngine.UI.Image GetAvailableBloodStain()
    {
        foreach(var stain in m_BloodStainBank)
        {
            if (!m_Shown.Contains(stain))
            {
                m_Shown.AddLast(stain);
                return stain;
            }
        }

        {
            var stain = m_Shown.First.Value;
            m_Shown.RemoveFirst();
            m_Shown.AddLast(stain);
            return stain;
        }
    }

    void GiveBackStain(UnityEngine.UI.Image stain)
    {
        m_Shown.Remove(stain);
    }

    void OnHealthChanged(object sender, float oldHealth, float newHealth)
    {
        var damage = oldHealth - newHealth;
        if (damage <= 0)
            return;

        var stain = GetAvailableBloodStain();
        var alpha = Mathf.Lerp(0, 1, damage / HeavyDamageIndicator);
        alpha = Mathf.Clamp01(alpha);
        stain.color = new Color(1, 1, 1, alpha);
    }
}
