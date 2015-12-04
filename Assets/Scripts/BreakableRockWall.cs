using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BreakableRockWall : BreakableRock
{
	// Use this for initialization
	protected override void Start ()
    {
        base.Start();

        m_forwardOut = transform.up;
	}
	
	// Update is called once per frame
    protected override void Update()
    {
        base.Update();
	}

    protected override void updateNotFrozenYet()
    {
        Ray ray = new Ray(transform.position - transform.up * (m_size.y / 2 - m_baseSize.y), -transform.up);
        RaycastHit hit;
        bool rayCast = Physics.Raycast(ray, out hit, 10);

        if (rayCast && hit.transform.gameObject.name.Contains("Terrain"))
        {
            m_rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            m_notFrozen = false;
        }
    }
}
