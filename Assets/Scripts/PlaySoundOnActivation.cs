using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource), typeof(TargetTrigger))]
public class PlaySoundOnActivation : MonoBehaviour {

    private AudioSource m_source = null;

	// Use this for initialization
	void Start () {
        m_source = GetComponent<AudioSource>();
        var trigger = GetComponent<TargetTrigger>();
        if(trigger)
        {
            trigger.ActiveChanged += OnActiveChanged;
        }
	}

    void OnActiveChanged(object _sender, bool _active)
    {
        if (_active)
            m_source.Play();
    }

}
