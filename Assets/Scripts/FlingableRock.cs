using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlingableRock : MonoBehaviour
{
    public float m_minSpeedStop = 1.0f;
    public float m_minAngularSpeedStop = 1.0f;
    public float m_earthFriction;
    float m_forceForward;
    float m_forceUp;
    public float m_forceStabilizer;

    Vector3 m_gravityForce;
    Vector3 m_size;
    float m_heightToReach;
    Vector3 m_forceTotal;
    Vector3 m_forward;

    public float m_minVelocityDestruction = 3.0f;
    public float m_collisionExplosionForce = 50.0f;
    public float m_collisionExplosionRadius = 50.0f;

    public float m_spawningHeightOffset { get; set; }
    bool m_risingStarted;
    bool m_risingDone;
    bool m_flingDone;
    bool m_isUnderground;
    bool m_wasUnderground;

    Rigidbody m_rigidBody;
    Collider m_collider;

    public GameObject m_smokeStartToMove;
    public GameObject m_smokeCollide;

    public GameObject m_user { get; set; }
	private AttackLauncher m_launcher { get; set; }

    public bool m_alreadyInTheWorld = false;

    List<Vector3> m_previousPos = new List<Vector3>();

    bool fire = false;
    bool fireCheat = false;
    BulletLauncher launcher;

    // 2700 is the average density of a rock Cf. http://www.les-mathematiques.net/phorum/read.php?2,49845
    float m_density = 2700;

	public float getHeightToReach() {
		return m_heightToReach;
	}

    void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
    }

    void Start()
    {
        updateSize();
        m_gravityForce = m_rigidBody.mass * Physics.gravity;
        m_previousPos.Add(transform.position);
        m_previousPos.Add(transform.position);
        if (m_alreadyInTheWorld)
            setStateAvailable();
    }

    // Use this for initialization
    // init() method has been created instead of Start() method to be able to
    // initialize the bullet when it is used again by a character
    virtual public void init(AttackLauncher _launcher, float _forceUp, float _forceForward)
    {
		m_launcher = _launcher;

//         if (_buttonToWatch.Contains("Cheat"))
//             fireCheat = true;
//         m_buttonToWatch = _buttonToWatch;
        m_risingStarted = false;
        m_risingDone = false;
        m_flingDone = false;
        m_isUnderground = true;
        if (launcher)
            m_heightToReach = launcher.transform.position.y + m_size.y;
        else if (m_user != null)
			m_heightToReach = m_user.transform.position.y + m_size.y;
		else
			m_heightToReach = 1f + m_size.y;
        m_forceUp = _forceUp;
        m_forceForward = _forceForward;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
//         float volume = m_rigidBody.mass / m_density;
//         float radius = 3 * volume / (4 * Mathf.PI);
//         m_rigidBody.AddExplosionForce(, m_rigidBody.position, radius);

        if (!collision.gameObject.GetComponent<Terrain>())
        {
            Ray ray = new Ray(m_previousPos[1], transform.position - m_previousPos[1]);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.DrawLine(ray.origin, hit.point, Color.yellow);
//                 GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//                 go.transform.localScale /= 5;
//                 go.transform.position = transform.position;
//                 go.GetComponent<MeshRenderer>().material.color = Color.blue;
//                 go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//                 go.transform.localScale /= 5;
//                 go.transform.position = m_previousPos[0];
//                 go.GetComponent<MeshRenderer>().material.color = Color.cyan;
//                 go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//                 go.transform.localScale /= 5;
//                 go.transform.position = m_previousPos[1];
//                 go.GetComponent<MeshRenderer>().material.color = Color.green;
//                 go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
//                 go.transform.localScale /= 5;
//                 go.transform.position = hit.point;
//                 go.GetComponent<MeshRenderer>().material.color = Color.yellow;
                Vector3 newPos = hit.point - new Vector3(m_size.x * m_rigidBody.velocity.normalized.x / 2,
                                                             m_size.y * m_rigidBody.velocity.normalized.y / 2,
                                                             m_size.z * m_rigidBody.velocity.normalized.z / 2);
                //Debug.DrawLine(hit.point, newPos, Color.white);
                transform.position = newPos;
                m_previousPos[0] = newPos;

                
                if (!collision.gameObject.GetComponent<Rigidbody>())
                {
                    CharacterMovement collidingObject = collision.gameObject.GetComponent<CharacterMovement>();
                    if (collidingObject)
                    {
                        Vector3 vect = getVelocity() - collidingObject.getVelocity();

                        Vector3 velocity1Final = (collidingObject.m_mass / (getMass() + collidingObject.m_mass)) * vect;
                        velocity1Final = velocity1Final.magnitude * collision.contacts[0].normal;

                        Vector3 velocity2Final = (-getMass() / (getMass() + collidingObject.m_mass)) * vect;
                        velocity2Final = velocity2Final.magnitude * -collision.contacts[0].normal;

                        Debug.DrawRay(collidingObject.transform.position, collidingObject.getVelocity(), Color.blue);
                        Debug.DrawRay(transform.position, getVelocity(), Color.green);
                        Debug.DrawRay(collidingObject.transform.position, velocity2Final, Color.cyan);
                        Debug.DrawRay(transform.position, velocity1Final, Color.red);
                        //UnityEditor.EditorApplication.isPaused = true;

                        setVelocity(velocity1Final);
                        collidingObject.setVelocity(velocity2Final);
                        // 
                        //                     collidingObject.setOnControllerColliderHitAlreadyCalled();
                    }
                }
            }
        }

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

		fire = m_launcher.isKey();

        m_forceTotal = m_gravityForce;

        bool heightReached = transform.position.y >= m_heightToReach;

        // Update Underground
        {
            RaycastHit hit;
            m_isUnderground = Physics.Raycast(m_collider.bounds.center, Vector3.up, out hit, 50)
                              && hit.collider.gameObject.name.Contains("Terrain");

            if (!m_isUnderground && m_wasUnderground)
            {
                Instantiate(m_smokeStartToMove, transform.position, Quaternion.identity);
                Physics.IgnoreLayerCollision(gameObject.layer, Manager.getManager().m_terrain.gameObject.layer, false);
            }
        }

        if (!m_risingStarted && !heightReached && m_user != null)
        {
            rise();
            m_risingStarted = true;
        }
        else if (!m_risingDone && !heightReached && (!launcher && fire || launcher && fireCheat))
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
        else if (m_risingDone && !m_flingDone)
        {
            if (fire)
                stabilize();
            else
            {
                m_rigidBody.velocity = new Vector3(m_rigidBody.velocity.x, 0.0f, m_rigidBody.velocity.z);

                Ray ray = m_user.GetComponent<AttackLauncher>().getAimRay();
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

                m_forward = hit.point - transform.position;
                m_forward.Normalize();

                m_forceTotal += m_forward * m_forceForward * getDistanceRatio();
                stabilize();
                m_flingDone = true;
                m_user = null;
                launcher = null;
            }
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
        m_previousPos[1] = m_previousPos[0];
        m_previousPos[0] = transform.position;
    }

    bool isGrounded()
    {
        return Physics.Raycast(m_collider.bounds.center, -Vector3.up, m_collider.bounds.extents.y + 0.1f);
    }

    bool isOnTheSameGroundOfTheUser()
    {
        RaycastHit hit;
        if (m_user != null
            && Physics.Raycast(m_collider.bounds.center, -Vector3.up, out hit, m_collider.bounds.extents.y + 0.1f))
        {
            string thisName = hit.collider.gameObject.name;
			BasicMovement movement = m_user.GetComponent<BasicMovement>();
			GameObject currentGround = movement.getCurrentGround();
			if (currentGround == null) return false;
            string thatName = currentGround.name;
//             Debug.Log("thisName=" + thisName);
//             Debug.Log("thatName=" + thatName);
            return thisName.Contains(thatName);
        }
        return false;
    }

    bool isOnTheSameGroundOfTheUser(CharacterMovementEarth _user)
    {
        RaycastHit hit;
        if (_user != null
            && Physics.Raycast(m_collider.bounds.center, -Vector3.up, out hit, m_collider.bounds.extents.y + 0.1f))
        {
            string thisName = hit.collider.gameObject.name;
			GameObject currentGround = _user.getCurrentGround();
			if (currentGround == null) return false;
            string thatName = currentGround.name;
            //             Debug.Log("thisName=" + thisName);
            //             Debug.Log("thatName=" + thatName);
            return thisName.Contains(thatName);
        }
        return false;
    }

    public void fling(AttackLauncher _launcher, float _forceUp, float _forceForward, bool _heightReached)
    {
		m_launcher = _launcher;
        m_risingDone = _heightReached;
        m_risingStarted = _heightReached;
        m_flingDone = false;
        m_forceUp = _forceUp;
        m_forceForward = _forceForward;
        Instantiate(m_smokeStartToMove, transform.position, Quaternion.identity);

        updateSize();

        if (_heightReached || /*m_user.transform.position.y < transform.position.y &&*/ !isOnTheSameGroundOfTheUser())
            m_heightToReach = transform.position.y;
        else
            m_heightToReach = transform.position.y + m_size.y;
    }

	public void setUser(GameObject user) {
		if (user == null) {
			Debug.Log("OK");
		}

		m_user = user;
	}

    /*public void setUser(string _playerID)
    {
        if (_playerID.Contains("FakePlayer"))
        {
            launcher = GameObject.Find(_playerID).GetComponent<BulletLauncher>();
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");

		foreach (GameObject go in gos)
        {
			if (go.GetComponent<CharacterMovement>() != null && go.GetComponent<CharacterMovement>().m_username.Equals(_playerID))
            {
				m_user = go.GetComponent<CharacterMovementEarth>();
				break;
			}
        }
    }*/

	/*if (go.GetComponent<BasicAI>() != null && go.GetComponent<CharacterMovement>().m_username.Equals(_playerID)) {
		m_user = go.GetComponent<CharacterMovementEarth>();
		break;
	}*/

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

        float volume = MeshVolumeHelper.VolumeOfObject(gameObject);
        m_rigidBody.mass = volume * m_density;
    }

    void rise()
    {
        m_forceTotal += Vector3.up * m_forceUp * getDistanceRatio();
    }

    void stabilize()
    {
        fireCheat = false;

        if (m_rigidBody.velocity.x > 0.05f || m_rigidBody.velocity.y > 0.05f)
        {
            Vector3 force = m_rigidBody.mass * -m_rigidBody.velocity / 0.05f;
            force = new Vector3(force.x, 0, force.z);
            if (force.magnitude >= m_forceForward)
                m_forceTotal += m_forward * m_forceForward;
            else
                m_forceTotal += force;
        }
        else
        {
            Vector3 force = m_rigidBody.mass * (-m_rigidBody.velocity / 0.05f);
            m_forceTotal += new Vector3(force.x, 0, force.z) * getDistanceRatio();
        }

        if (transform.position.y < m_heightToReach || transform.position.y > m_heightToReach + 0.05f)
        {
            Vector3 v1 = (new Vector3(0, m_heightToReach, 0) - new Vector3(0, transform.position.y, 0)) / 0.05f;
            Vector3 force = m_rigidBody.mass * (v1 - m_rigidBody.velocity) / 0.05f;
            m_forceTotal += (Vector3.up * force.y - m_gravityForce) * getDistanceRatio();
        }
        else
        {
            Vector3 force = m_rigidBody.mass * (-m_rigidBody.velocity / 0.05f);
            m_forceTotal += (Vector3.up * force.y - m_gravityForce) * getDistanceRatio();
        }
    }

    float getDistanceRatio()
    {
        float ratio;
        if (launcher)
            ratio = launcher.m_OffsetForwardEarth / Vector3.Distance(transform.position, launcher.transform.position);
		else if (m_user == null)
			return 999f;
        else
	        ratio = m_user.GetComponent<EarthAttack>().m_OffsetForwardEarth / Vector3.Distance(transform.position, m_user.transform.position);
        
        return Mathf.Min(4 * ratio, 1);
    }

    float getDistanceRatio(GameObject _user)
    {
        float ratio = _user.GetComponent<EarthAttack>().m_OffsetForwardEarth / Vector3.Distance(transform.position, _user.transform.position);
        return Mathf.Min(4 * ratio, 1);
    }
    
    float getDistanceRatio(BulletLauncher _user)
    {
        float ratio = _user.m_OffsetForwardEarth / Vector3.Distance(transform.position, _user.transform.position);
        return Mathf.Min(4 * ratio, 1);
    }

    void setStateAvailable()
    {
        m_heightToReach = transform.position.y;
        m_risingStarted = true;
        m_risingDone = true;
        m_flingDone = true;
        m_user = null;
        launcher = null;
    }

    public bool canRiseInMinTime(float timeToRise, GameObject user)
    {
        if (!isOnTheSameGroundOfTheUser())
            m_heightToReach = transform.position.y;
        else
            m_heightToReach = transform.position.y + m_size.y;

        int nbFrameToDo = (int) (timeToRise / Time.deltaTime);
        float timePerFrame = timeToRise / nbFrameToDo;
        Vector3 force = m_gravityForce + Vector3.up * m_forceUp * getDistanceRatio(user);
        Vector3 acceleration = force / m_rigidBody.mass;

        float heightTraveled = 0;
        float speed = 0;

        for (int i = 0; i < nbFrameToDo; ++i)
        {
            speed += acceleration.y * timePerFrame;
            heightTraveled += speed * timePerFrame;
        }

        return transform.position.y + heightTraveled >= m_heightToReach;
    }

    public bool canRiseInMinTime(float timeToRise, BulletLauncher user)
    {
        if (!isOnTheSameGroundOfTheUser())
            m_heightToReach = transform.position.y;
        else
            m_heightToReach = transform.position.y + m_size.y;

        int nbFrameToDo = (int)(timeToRise / Time.deltaTime);
        float timePerFrame = timeToRise / nbFrameToDo;
        Vector3 force = m_gravityForce + Vector3.up * m_forceUp * getDistanceRatio(user);
        Vector3 acceleration = force / m_rigidBody.mass;

        float heightTraveled = 0;
        float speed = 0;

        for (int i = 0; i < nbFrameToDo; ++i)
        {
            speed += acceleration.y * timePerFrame;
            heightTraveled += speed * timePerFrame;
        }

        return transform.position.y + heightTraveled >= m_heightToReach;
    }

    public Vector3 getVelocity()
    {
        return m_rigidBody.velocity;
    }

    public void setVelocity(Vector3 _velocity)
    {
        m_rigidBody.velocity = _velocity;
    }

    public float getMass()
    {
        return m_rigidBody.mass;
    }
}
