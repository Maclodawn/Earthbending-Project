using UnityEngine;
using System.Collections;

public class BasicRockBullet : MonoBehaviour
{
    Manager m_manager;

    public float m_minSpeedStop = 1.0f;
    public float m_minAngularSpeedStop = 1.0f;
    public float m_earthFriction = 40;
    public float m_forceForward = 3000;
    public float m_forceUp = 500;
    public float m_forceStabilizer = 10;
    public float m_heightOffset = 0.7f;

    public float m_minVelocityDestruction = 3.0f;
    public float m_collisionExplosionForce = 50.0f;
    public float m_collisionExplosionRadius = 50.0f;

    float m_height;
    public float m_spawningHeightOffset { get; set; }
    bool m_risingStarted;
    bool m_risingDone;
    bool m_flingDone;
    bool m_isSpawning;

    private Rigidbody m_rigidBody;
    private Collider m_collider;

    public GameObject m_smokeStartToMove;
    public GameObject m_smokeCollide;

    public CharacterMovement m_user { get; set; }

	// Use this for initialization
	public void init (Manager _manager)
    {
        m_manager = _manager;
        m_rigidBody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        m_isSpawning = true;
        m_height = transform.position.y;
        m_risingDone = false;
        m_risingStarted = false;
        m_flingDone = false;
        Instantiate(m_smokeStartToMove, transform.position, Quaternion.identity);
        m_collider.enabled = false;
	}

    void OnCollisionEnter(Collision col)
    {
        m_rigidBody.AddExplosionForce(m_collisionExplosionForce, m_rigidBody.position, m_collisionExplosionRadius);

        if (m_rigidBody.velocity.magnitude >= m_minVelocityDestruction)
        {
            //rigidbody.AddExplosionForce(3.0f, rigidbody.position, 3.0f);
            Instantiate(m_smokeCollide, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Destroy(this.gameObject);
        }

        bool heightReached = false;
        if (m_isSpawning)
            heightReached = transform.position.y >= m_height + m_heightOffset - m_spawningHeightOffset;
        else
            heightReached = transform.position.y >= m_height + m_heightOffset;

        if (!m_risingStarted && !heightReached)
        {
            m_rigidBody.AddForce(Vector3.up * m_forceUp);
            m_risingStarted = true;
        }
        else if (!m_risingDone && heightReached)
        {
            m_rigidBody.velocity = new Vector3(m_rigidBody.velocity.x, 0.0f, m_rigidBody.velocity.z);
            m_collider.enabled = true;
            m_risingDone = true;
        }
        else if (m_risingDone && !m_flingDone && Input.GetButton("Fire1") && m_rigidBody.velocity.y < 0)
        {
            m_rigidBody.AddForce(Vector3.up * m_forceStabilizer);
        }
        else if (!m_flingDone && m_risingDone && !Input.GetButton("Fire1"))
        {
            m_rigidBody.velocity = new Vector3(m_rigidBody.velocity.x, 0.0f, m_rigidBody.velocity.z);

            Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, 5000))
            {
                hit.point = ray.direction * 5000;
            }

            Debug.DrawRay(ray.origin, hit.point);

            transform.forward = hit.point - transform.position;
            m_rigidBody.AddForce(transform.forward * m_forceForward);
            m_flingDone = true;

            m_user = null;
        }
	}

    void LateUpdate()
    {
        if (m_risingDone && isGrounded())
        {
            if (m_rigidBody.velocity.magnitude > m_minSpeedStop)
                m_rigidBody.AddForce(-m_rigidBody.velocity.normalized * m_earthFriction);
            else
                m_rigidBody.velocity = Vector3.zero;

            if (m_rigidBody.angularVelocity.magnitude > m_minAngularSpeedStop)
                m_rigidBody.AddForce(-m_rigidBody.angularVelocity.normalized * m_earthFriction);
            else
                m_rigidBody.angularVelocity = Vector3.zero;
        }
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, m_collider.bounds.extents.y + 0.1f);
    }

    public void fling()
    {
        m_isSpawning = false;
        m_height = transform.position.y;
        m_risingDone = false;
        m_risingStarted = false;
        m_flingDone = false;
        Instantiate(m_smokeStartToMove, transform.position, Quaternion.identity);
        m_collider.enabled = false;
    }
    
    public void setUser(string _playerID)
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in gos)
        {
            if (go.GetComponent<CharacterMovement>().m_username.Equals(_playerID))
            {
                m_user = go.GetComponent<CharacterMovement>();
                break;
            }
        }
    }
}
