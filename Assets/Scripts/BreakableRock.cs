using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BreakableRock : MonoBehaviour
{
    bool m_isSpawning = true;
    protected bool m_notFrozen = true;
    [SerializeField]
    float m_timeToGoOut;
    protected Vector3 m_forwardOut;

    protected Vector3 m_size;
    protected Vector3 m_baseSize;

    protected Rigidbody m_rigidBody;
    protected BoxCollider m_boxCollider;

    [SerializeField]
    List<GameObject> m_pieceList;

#pragma warning disable 0414 // Assigned but never used as it is used only in children classes
    Transform m_base;
#pragma warning restore 0414

    // Use this for initialization
    protected virtual void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();

        m_boxCollider = transform.GetComponentInChildren<BoxCollider>();

        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform tmpTransform = transform.GetChild(i);
            MeshRenderer tmpMeshRenderer = tmpTransform.GetComponent<MeshRenderer>();
            if (tmpMeshRenderer == null)
                continue;

            if (tmpTransform.gameObject.name.Contains("Base"))
            {
                m_base = tmpTransform;
                m_baseSize = tmpMeshRenderer.bounds.size;
            }
            m_size += tmpMeshRenderer.bounds.size;
        }

        float volume = MeshVolumeHelper.VolumeOfObject(gameObject);
        // 2700 is the average density of a rock Cf. http://www.les-mathematiques.net/phorum/read.php?2,49845
        m_rigidBody.mass = volume * 2700;
    }

    // Update is called once per frame
    protected virtual void Update()
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
            updateNotFrozenYet();
        }
    }

    protected virtual void updateNotFrozenYet()
    {}

    public void breakRock(string _username, string _buttonToWatch, float _forceUp, float _forceForward)
    {
        if (transform.childCount <= 1)
            return;

        Transform child = transform.GetChild(0);
        if (!child.name.Contains("Part"))
            return;

        MeshRenderer childMeshRenderer = child.GetComponent<MeshRenderer>();

        float centerRatio = childMeshRenderer.bounds.size.y / (2 * m_size.y);
        m_boxCollider.center -= new Vector3(0, centerRatio, 0);

        float sizeRatio = 1 - childMeshRenderer.bounds.size.y / m_size.y;
        m_boxCollider.size = new Vector3(m_boxCollider.size.x, sizeRatio, m_boxCollider.size.z);

        m_size -= childMeshRenderer.bounds.size;

        if (m_pieceList.Count == 0)
            return;

        Object obj = Instantiate(m_pieceList[0], child.position + Vector3.up * 0.1f, child.rotation);
        m_pieceList.RemoveAt(0);
        GameObject gameObject = (GameObject)obj;
        scaleIt(gameObject);
        FlingableRock flingableRock = gameObject.GetComponent<FlingableRock>();
        Destroy(child.gameObject);
        flingableRock.setUser(_username);
        flingableRock.fling(_buttonToWatch, _forceUp, _forceForward, true);
    }

    protected virtual void scaleIt(GameObject _gameObject)
    {
        // Do nothing
    }
}
