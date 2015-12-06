using UnityEngine;
using System.Collections;

public class TargetTrigger : MonoBehaviour {

    public float ActivationSpeed = 5;

    public delegate void ActiveChangedEventHandler(object _sender, bool _active);
    public event ActiveChangedEventHandler ActiveChanged;

    private bool m_active;

    public bool Active
    {
        get { return m_active; }
        set 
        {
            if(m_active != value)
            {
                m_active = value;
                OnActiveChanged(m_active);
            }
        }
    }

    public float DeactivationTime = 0;
    private float m_deactivationTimer = 0;
    
    void Start()
    {
        Active = false;
    }

    // Update is called once per frame
	void Update () {
	    if(DeactivationTime > 0 && Active)
        {
            m_deactivationTimer += Time.deltaTime;
            if(m_deactivationTimer >= DeactivationTime)
            {
                Active = false;
                m_deactivationTimer = 0;
            }
        }
	}
    
    void OnCollisionEnter(Collision _collision)
    {
        if(_collision.relativeVelocity.sqrMagnitude >= ActivationSpeed * ActivationSpeed)
        {
            Active = true;
        }
    }

    protected void OnActiveChanged(bool _active)
    {
        if (ActiveChanged != null)
            ActiveChanged(this, _active);
    }
}
