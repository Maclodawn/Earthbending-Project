using UnityEngine;
using System.Collections;

public class BreakableRockWall : BreakableRock
{
    bool m_isSpawning = true;
    bool m_notFrozen = true;
    [SerializeField]
    float m_timeToGoOut;

    float m_ySize;
    float m_yBaseSize;

    Rigidbody m_rigidbody;
    BoxCollider m_boxCollider;

	// Use this for initialization
	void Start ()
    {
        m_rigidbody = GetComponent<Rigidbody>();

        m_boxCollider = GetComponent<BoxCollider>();
        m_boxCollider.enabled = false;

        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform tmpTransform = transform.GetChild(i);
            MeshRenderer tmpMeshRenderer = tmpTransform.GetComponent<MeshRenderer>();
            if (tmpTransform.gameObject.name.Contains("Base"))
                m_yBaseSize = tmpMeshRenderer.bounds.size.y;
            m_ySize += tmpMeshRenderer.bounds.size.y;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Destroy(this.gameObject);
        }

	    if (m_isSpawning)
        {
            float forceUp = m_rigidbody.mass * m_ySize / (m_timeToGoOut * m_timeToGoOut);
            m_rigidbody.AddForce(transform.up * forceUp);

            m_isSpawning = false;
        }
        else if (m_notFrozen)
        {
            Ray ray = new Ray(transform.position - transform.up * (m_ySize / 2 - m_yBaseSize), -transform.up);
            RaycastHit hit;
            bool rayCast = Physics.Raycast(ray, out hit, 10);
            if (rayCast && hit.transform.gameObject.name.Contains("Terrain"))
            {
                m_rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                m_notFrozen = false;
                m_boxCollider.enabled = true;
            }
        }
	}
}
