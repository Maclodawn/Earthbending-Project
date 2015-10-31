using UnityEngine;
using System.Collections;

public class BreakableRockWall : BreakableRock
{
    bool m_isSpawning = true;
    bool m_notFrozen = true;
    public float m_forceUp;

    Rigidbody m_rigidbody;

	// Use this for initialization
	void Start ()
    {
        m_rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (m_isSpawning)
        {
            m_rigidbody.AddForce(transform.up * m_forceUp);

            m_isSpawning = false;
        }

        if (m_notFrozen)
        {
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10) && hit.transform.gameObject.name.Contains("Terrain"))
                m_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
	}
}
