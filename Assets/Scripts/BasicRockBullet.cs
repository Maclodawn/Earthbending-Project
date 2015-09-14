using UnityEngine;
using System.Collections;

public class BasicRockBullet : MonoBehaviour {

    public GameObject m_managerObject;
    Manager m_manager;

    public Vector3 m_acceleration = Vector3.zero;
    public Vector3 m_velocity = Vector3.zero;

    public float m_earthFriction = 40;
    public float m_forceForward = 3000;
    public float m_forceUp = 500;
    public float m_forceStabilizer = 10;
    public float m_heightOffset = 0.7f;

    public float m_minDestruction = 3.0f;
    public float m_collisionExplosionForce = 50.0f;
    public float m_collisionExplosionRadius = 50.0f;

    public float m_height { get; set; }
    public float m_spawningHeightOffset { get; set; }
    bool m_applyEarthFriction;
    bool m_risingStarted;
    bool m_risingDone;
    bool m_flingDone;
    public bool m_isSpawning { get; set; }

    public GameObject m_smokeStartToMove;
    public GameObject m_smokeCollide;

    public Player_Movement m_user { get; set; }

	// Use this for initialization
	void Start ()
    {
        m_manager = m_managerObject.GetComponent<Manager>();
        m_manager.m_bulletList.Add(this);
        m_isSpawning = true;
        m_height = transform.position.y;
        m_risingDone = false;
        m_risingStarted = false;
        m_flingDone = false;
        m_applyEarthFriction = true;
        Instantiate(m_smokeStartToMove, transform.position, Quaternion.identity);
        GetComponent<Collider>().enabled = false;
	}

    void OnCollisionEnter(Collision col)
    {
        GetComponent<Rigidbody>().AddExplosionForce(m_collisionExplosionForce, GetComponent<Rigidbody>().position, m_collisionExplosionRadius);

        if (GetComponent<Rigidbody>().velocity.magnitude >= m_minDestruction)
        {
            //rigidbody.AddExplosionForce(3.0f, rigidbody.position, 3.0f);
            Destroy(this.gameObject);
            m_manager.m_bulletList.Remove(this);
            Instantiate(m_smokeCollide, transform.position, Quaternion.identity);
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            /*Network.*/Destroy(this.gameObject);
            m_manager.m_bulletList.Remove(this);
        }

        bool heightReached = false;
        if (m_isSpawning)
            heightReached = transform.position.y >= m_height + m_heightOffset - m_spawningHeightOffset;
        else
            heightReached = transform.position.y >= m_height + m_heightOffset;

        if (!m_risingStarted && !heightReached)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * m_forceUp);
            m_risingStarted = true;
        }
        else if (!m_risingDone && heightReached)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0.0f, GetComponent<Rigidbody>().velocity.z);
            GetComponent<Collider>().enabled = true;
            m_risingDone = true;
        }
        else if (m_risingDone && !m_flingDone && Input.GetMouseButton(0) && GetComponent<Rigidbody>().velocity.y < 0)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * m_forceStabilizer);
        }
        else if (!m_flingDone && m_risingDone && !Input.GetMouseButton(0))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0.0f, GetComponent<Rigidbody>().velocity.z);

            Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, 5000))
            {
                hit.point = ray.direction * 5000;
            }

            Debug.DrawRay(ray.origin, hit.point);

            transform.forward = hit.point - transform.position;
            GetComponent<Rigidbody>().AddForce(transform.forward * m_forceForward);
            m_flingDone = true;

            m_user = null;
        }

        if (m_risingDone && isGrounded())
        {
            if (GetComponent<Rigidbody>().velocity.magnitude > 1)
            {
                GetComponent<Rigidbody>().AddForce(-GetComponent<Rigidbody>().velocity.normalized * m_earthFriction);
                m_applyEarthFriction = true;
            }
            else if (m_applyEarthFriction)
            {
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                m_applyEarthFriction = false;
            }
        }
	}

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + 0.1f);
    }

    public void fling()
    {
        m_isSpawning = false;
        m_height = transform.position.y;
        m_risingDone = false;
        m_risingStarted = false;
        m_flingDone = false;
        m_applyEarthFriction = true;
        Instantiate(m_smokeStartToMove, transform.position, Quaternion.identity);
        GetComponent<Collider>().enabled = false;
    }

    //     public void setUser(string _userID)
    //     {
    //         GetComponent<NetworkView>().RPC("IsUsedBy", RPCMode.All, _userID);
    //     }
    // 
    //     [RPC]
    //     void IsUsedBy(string _playerID)
    //     {
    //         GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
    //         foreach (GameObject go in gos)
    //         {
    //             if (go.GetComponent<PlayerMovement>().m_id.Equals(_playerID))
    //             {
    //                 m_user = go.GetComponent<PlayerMovement>();
    //                 break;
    //             }
    //         }
    //         //m_user = GameObject.FindGameObjectWithTag(_playerID).GetComponent<PlayerMovement>();
    //     }
}
