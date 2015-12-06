using UnityEngine;
using System.Collections;

public class IndicatorUpdate : MonoBehaviour {

    public Material ActiveMaterial;
    public Material InactiveMaterial;

    private TargetTrigger m_trigger = null;
    private Renderer m_indicator = null;

	// Use this for initialization
	void Start () {
        m_trigger = GetComponentInParent<TargetTrigger>();
        if (m_trigger)
        {
            m_trigger.ActiveChanged += OnActiveChanged;
            m_indicator = GetComponent<Renderer>();
        }
	}
	
    void OnActiveChanged(object _sender, bool _active)
    {
        m_indicator.material = _active ? ActiveMaterial : InactiveMaterial;
    }
}
