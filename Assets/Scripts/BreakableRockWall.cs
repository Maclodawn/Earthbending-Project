using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BreakableRockWall : BreakableRock
{
    bool m_isSpawning = true;
    bool m_notFrozen = true;
    [SerializeField]
    float m_timeToGoOut;

    float m_ySize;
    float m_yBaseSize;

    Rigidbody m_rigidBody;
    BoxCollider m_boxCollider;

    [SerializeField]
    List<GameObject> m_pieceList;

	// Use this for initialization
	void Start ()
    {
        m_rigidBody = GetComponent<Rigidbody>();

        m_boxCollider = GetComponent<BoxCollider>();

        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform tmpTransform = transform.GetChild(i);
            MeshRenderer tmpMeshRenderer = tmpTransform.GetComponent<MeshRenderer>();
            if (tmpTransform.gameObject.name.Contains("Base"))
                m_yBaseSize = tmpMeshRenderer.bounds.size.y;
            m_ySize += tmpMeshRenderer.bounds.size.y;
        }

        float volume = MeshVolumeHelper.VolumeOfObject(gameObject);
        // 2700 is the average density of a rock Cf. http://www.les-mathematiques.net/phorum/read.php?2,49845
        m_rigidBody.mass = volume * 2700;
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
            float forceUp = m_rigidBody.mass * m_ySize / (m_timeToGoOut * m_timeToGoOut);
            m_rigidBody.AddForce(transform.up * forceUp);

            m_isSpawning = false;
        }
        else if (m_notFrozen)
        {
            Ray ray = new Ray(transform.position - transform.up * (m_ySize / 2 - m_yBaseSize), -transform.up);
            RaycastHit hit;
            bool rayCast = Physics.Raycast(ray, out hit, 10);
            if (rayCast && hit.transform.gameObject.name.Contains("Terrain"))
            {
                m_rigidBody.constraints = RigidbodyConstraints.FreezeAll;
                m_notFrozen = false;
            }
        }
	}

    public void breakRock(string _username, string _buttonToWatch, float _forceUp, float _forceForward)
    {
        if (transform.childCount <= 1)
            return;
        
        Transform child = transform.GetChild(0);
        if (!child.name.Contains("WallEarth"))
            return;

        MeshRenderer childMeshRenderer = child.GetComponent<MeshRenderer>();

        float centerRatio = childMeshRenderer.bounds.size.y / (2 * m_ySize);
        m_boxCollider.center -= new Vector3(0, centerRatio, 0);

        float sizeRatio = 1 - childMeshRenderer.bounds.size.y / m_ySize;
        m_boxCollider.size = new Vector3(m_boxCollider.size.x, sizeRatio, m_boxCollider.size.z);

        m_ySize -= childMeshRenderer.bounds.size.y;
        
        if (m_pieceList.Count == 0)
            return;

        Object obj = Instantiate(m_pieceList[0], child.position + Vector3.up * 0.1f, child.rotation);
        m_pieceList.RemoveAt(0);
        GameObject gameObject = (GameObject) obj;
        FlingableRock flingableRock = gameObject.GetComponent<FlingableRock>();
        Destroy(child.gameObject);
        flingableRock.setUser(_username);

        flingableRock.fling(_buttonToWatch, _forceUp, _forceForward, true);
    }

    void OnCollisionEnter(Collision collision)
    {
        int toto = 0;
        toto++;
    }
}
