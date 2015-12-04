using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BreakableRockPillar : BreakableRock
{
    bool m_isSpawning = true;
    bool m_notFrozen = true;
    [SerializeField]
    float m_timeToGoOut;
    Vector3 m_forwardOut;

    Vector3 m_size;
    Vector3 m_baseSize;

    Rigidbody m_rigidBody;
    BoxCollider m_boxCollider;

    [SerializeField]
    List<GameObject> m_pieceList;

    // Use this for initialization
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();

        m_boxCollider = transform.GetComponentInChildren<BoxCollider>();
        m_boxCollider.enabled = false;

        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform tmpTransform = transform.GetChild(i);
            MeshRenderer tmpMeshRenderer = tmpTransform.GetComponent<MeshRenderer>();
            if (tmpMeshRenderer == null)
                continue;

            if (tmpTransform.gameObject.name.Contains("Base"))
                m_baseSize = tmpMeshRenderer.bounds.size;
            m_size += tmpMeshRenderer.bounds.size;
        }

        float angle = m_boxCollider.transform.eulerAngles.z * Mathf.PI / 180;
        float adjacent = transform.forward.magnitude;
        float hypothenus = adjacent / Mathf.Cos(angle);
        float opposite = hypothenus * Mathf.Sin(angle);
        Vector3 normal = Vector3.Cross(transform.forward, transform.right).normalized;

        m_forwardOut = transform.forward + normal * opposite;

        float y = m_size.y / 4 - m_baseSize.y;
        float x = m_size.x / 4;
        float z = m_size.z / 4;
        float xz = Mathf.Sqrt(x * x + z * z);
        transform.position -= m_forwardOut * Mathf.Sqrt(y * y + xz * xz);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Destroy(this.gameObject);
        }

        if (m_isSpawning)
        {
            float forceUp = m_rigidBody.mass * m_size.y / (m_timeToGoOut * m_timeToGoOut);
            m_rigidBody.AddForce(m_forwardOut * forceUp);

            m_isSpawning = false;
        }
        else if (m_notFrozen)
        {
            Ray ray = new Ray(transform.position - transform.up * (m_size.y * transform.localScale.x / 2 - m_baseSize.y), -transform.up);
            RaycastHit hit;
            bool rayCast = Physics.Raycast(ray, out hit, 10);

            if (rayCast && hit.transform.gameObject.name.Contains("Terrain"))
            {
                m_rigidBody.constraints = RigidbodyConstraints.FreezeAll;
                m_notFrozen = false;
                m_boxCollider.enabled = true;
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

        float centerRatio = childMeshRenderer.bounds.size.y / (2 * m_size.y);
        m_boxCollider.center -= new Vector3(0, centerRatio, 0);

        float sizeRatio = 1 - childMeshRenderer.bounds.size.y / m_size.y;
        m_boxCollider.size = new Vector3(m_boxCollider.size.x, sizeRatio, m_boxCollider.size.z);

        m_size -= childMeshRenderer.bounds.size;

        if (m_pieceList.Count == 0)
            return;

        Object obj = Instantiate(m_pieceList[0], child.position, child.rotation);
        GameObject gameObject = (GameObject)obj;
        FlingableRock flingableRock = gameObject.GetComponent<FlingableRock>();
        Destroy(child.gameObject);
        flingableRock.setUser(_username);
        flingableRock.fling(_buttonToWatch, _forceUp, _forceForward, true);
    }
}
