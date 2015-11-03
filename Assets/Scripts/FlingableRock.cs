using UnityEngine;
using System.Collections;

public class FlingableRock : MonoBehaviour
{
    public float m_minSpeedStop = 1.0f;
    public float m_minAngularSpeedStop = 1.0f;
    public float m_earthFriction;
    public float m_speedForward;
    public float m_ratioToRise;
    public float m_timeMinToRise;

    Vector3 m_gravityForce;
    Vector3 m_size;
    float m_timeToRise;
    float m_heightToReach;
    Vector3 m_forceTotal;

    public float m_minVelocityDestruction = 3.0f;
    public float m_collisionExplosionForce = 50.0f;
    public float m_collisionExplosionRadius = 50.0f;

    public float m_spawningHeightOffset { get; set; }
    protected bool m_risingStarted;
    protected bool m_risingDone;
    protected bool m_flingDone;
    protected bool m_isSpawning;
    bool m_isUnderground;
    bool m_wasUnderground;

    protected Rigidbody m_rigidBody;
    protected Collider m_collider;

    public GameObject m_smokeStartToMove;
    public GameObject m_smokeCollide;

    public CharacterMovement m_user { get; set; }

    protected string m_buttonToWatch = "";

    public bool m_alreadyInTheWorld = false;

    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
    }

    void Start()
    {
        updateSize();
        m_gravityForce = m_rigidBody.mass * Physics.gravity;
        if (m_alreadyInTheWorld)
            setStateAvailable();
    }

    // Use this for initialization
    // init() method has been created instead of Start() method to be able to
    // initialize the bullet when it is used again by a character
    virtual public void init(string _buttonToWatch)
    {
        m_buttonToWatch = _buttonToWatch;
        m_isSpawning = true;
        m_risingDone = false;
        m_risingStarted = false;
        m_flingDone = false;
        m_isUnderground = true;
        m_heightToReach = m_user.transform.position.y + m_size.y;
    }

    protected virtual void OnCollisionEnter(Collision col)
    {
        m_rigidBody.AddExplosionForce(m_collisionExplosionForce, m_rigidBody.position, m_collisionExplosionRadius);

        if (m_rigidBody.velocity.magnitude >= m_minVelocityDestruction)
        {
            Instantiate(m_smokeCollide, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    virtual protected void FixedUpdate()
    {
        // Cheat
        if (Input.GetKeyDown(KeyCode.A))
        {
            Destroy(this.gameObject);
        }

        m_forceTotal = m_gravityForce;

        bool heightReached = transform.position.y >= m_heightToReach;

        // Update Underground
        {
            RaycastHit hit;
            m_isUnderground = Physics.Raycast(transform.position, Vector3.up, out hit, 50)
                              && hit.collider.gameObject.name.Contains("Terrain");
            m_collider.enabled = !m_isUnderground;

            if (!m_isUnderground && m_wasUnderground)
                Instantiate(m_smokeStartToMove, transform.position, Quaternion.identity);
        }

        if (!m_risingStarted && !heightReached)
        {
            rise();
            m_risingStarted = true;
        }
        else if (!m_risingDone && !heightReached && m_buttonToWatch.Length != 0 && Input.GetButton(m_buttonToWatch))
        {
            rise();
        }
        else if (!m_risingDone && !heightReached)
        {
            setStateAvailable();
        }
        else if (!m_risingDone && heightReached)
        {
            m_rigidBody.velocity = new Vector3(m_rigidBody.velocity.x, 0.0f, m_rigidBody.velocity.z);
            m_risingDone = true;
        }
        else if (m_risingDone && !m_flingDone && m_buttonToWatch.Length != 0 && Input.GetButton(m_buttonToWatch))
        {
            if (m_rigidBody.velocity.y < 0)
                m_rigidBody.velocity = new Vector3(m_rigidBody.velocity.x, 0.0f, m_rigidBody.velocity.z);
            m_forceTotal += -m_gravityForce;
        }
        else if (!m_flingDone && m_risingDone && m_buttonToWatch.Length != 0 && !Input.GetButton(m_buttonToWatch))
        {
            m_rigidBody.velocity = new Vector3(m_rigidBody.velocity.x, 0.0f, m_rigidBody.velocity.z);

            Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
            RaycastHit hit = new RaycastHit();
            RaycastHit[] hitList = Physics.RaycastAll(ray, 5000);
            
            // Do not take in account this.gameobject as aim
            if (hitList.Length < 1)
                hit.point = ray.direction * 5000;
            else if (!hitList[0].collider.gameObject.name.Contains(gameObject.name))
                hit = hitList[0];
            else if (hitList.Length < 2)
                hit.point = ray.direction * 5000;
            else
                hit = hitList[1];

            Vector3 forward = hit.point - transform.position;
            forward.Normalize();
            // We need the FlingableRock to reach its forward speed instantly (0.1f)
            float forceForward = m_rigidBody.mass * m_speedForward / 0.1f;
            m_forceTotal += forward * forceForward;
            m_forceTotal += -m_gravityForce;
            m_flingDone = true;

            m_user = null;
        }

        if (m_risingDone && isGrounded())
        {
            if (m_rigidBody.velocity.magnitude > m_minSpeedStop)
                m_forceTotal += -m_rigidBody.velocity.normalized * m_earthFriction;
            else
                m_rigidBody.velocity = Vector3.zero;

            if (m_rigidBody.angularVelocity.magnitude > m_minAngularSpeedStop)
                m_forceTotal += -m_rigidBody.angularVelocity.normalized * m_earthFriction;
            else
                m_rigidBody.angularVelocity = Vector3.zero;
        }

        m_rigidBody.AddForce(m_forceTotal);
        m_wasUnderground = m_isUnderground;
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, m_collider.bounds.extents.y + 0.1f);
    }

    public void fling(string _buttonToWatch)
    {
        m_buttonToWatch = _buttonToWatch;
        m_isSpawning = false;
        m_risingDone = false;
        m_risingStarted = false;
        m_flingDone = false;
        Instantiate(m_smokeStartToMove, transform.position, Quaternion.identity);
        if (m_user.transform.position.y < transform.position.y && isGrounded())
            m_heightToReach = transform.position.y + 0.5f; // Ratio between user muscles and the FlingableRock mass
        else if (m_user.transform.position.y < transform.position.y)
            m_heightToReach = transform.position.y;
        else
            m_heightToReach = m_user.transform.position.y + m_size.y;
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

    void updateSize()
    {
        m_size = Vector3.zero;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer)
            m_size = meshRenderer.bounds.size;
        else
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                meshRenderer = transform.GetChild(i).GetComponent<MeshRenderer>();
                if (!meshRenderer)
                    continue;

                m_size += meshRenderer.bounds.size;
            }
        }

        float volume = m_size.x * m_size.y * m_size.z;
        // 2700 is the average density of a rock Cf. http://www.les-mathematiques.net/phorum/read.php?2,49845
        m_rigidBody.mass = volume * 2700;
        m_timeToRise = m_ratioToRise * m_rigidBody.mass;
        if (m_timeToRise < m_timeMinToRise)
            m_timeToRise = m_timeMinToRise;
    }

    void rise()
    {
        float forceUp = m_rigidBody.mass * (m_heightToReach - transform.position.y) / (m_timeToRise * m_timeToRise);
        m_forceTotal += Vector3.up * forceUp;
        m_forceTotal += -m_gravityForce;
    }

    void setStateAvailable()
    {
        m_heightToReach = transform.position.y;
        m_isSpawning = false;
        m_risingStarted = true;
        m_risingDone = true;
        m_flingDone = true;
        m_user = null;
    }
}
