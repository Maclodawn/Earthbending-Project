using UnityEngine;
using System.Collections;

public abstract class BasicMovement : MonoBehaviour {
	
	protected CharacterController m_controller = null;
	protected Vector2 input = Vector2.zero;
	protected bool crouch = false, sprint = false, jump = false, dodge = false;
	private AttackLauncher launcher;

	// --- movement parameters

	protected float m_currentMoveSpeed = 1.0f;		// vitesse actuelle : mise à jour
	public float m_runSpeed = 7.0f;					// vitesse de course
	public float m_sprintSpeed = 14.0f;				// vitesse de sprint
	public float m_crouchSpeed = 1.0f;				// vitesse accroupi
	public float m_jumpSpeed = 20.0f;				// vitesse quand on saute (utilisée seulement en Y)
	
	public float m_smoothMovement = 0.1f;			// smoothage du mouvement
	protected float m_gravity = 1.0f;				// gravité : pour quand on saute
	protected float m_yVelocity = 0.0f;				// vélocité en Y par défaut
	protected float m_forwardSpeed = 0;				// vitesse en avant
	protected float m_rightSpeed = 0;				// vitesse sur le côté

	public bool m_crouched = false;					// pour savoir s'il est accroupi
	
	protected float m_coolDownDodge = 0.15f;		// compteur de temps s'est écoulé depuis le début de l'anim
	public float m_coolDownDodgeTimer = 0.5f;		// combien de temps dure un dodge
	public float m_dodgeSpeed = 30;					// vitesse du dodge
	protected bool m_dodging = false;				// savoir si on est en train de dodger ou pas
	public float m_smoothMovementDodge = 1;			// tweak sur le dodge
	public float m_cooldownBeforeDodge = 1;			// temps à attendre avant de dodge
	protected float m_cooldownBeforeDodgeTimer = 1;	// temps qui s'est écoulé depuis dernier dodge
	protected bool m_ableToDodge = true;			// savoir si on peut dodge

    protected bool m_pause;

    protected Collider m_collider;

    bool m_tookAHit = false;
    Vector3 m_velocityHit;
    [SerializeField]
    float m_speedFrictionHit;
    
    //    bool m_onControllerColliderHitAlreadyCalled = false;

    // ---
	
	public void Start() {
		m_controller = GetComponent<CharacterController>();
        m_collider = GetComponent<Collider>();
        m_currentMoveSpeed = m_runSpeed;
		m_smoothMovementDodge = m_smoothMovement;
    }

	public void Update() {
		updateInput();
		workFromInput();
	}

	public void FixedUpdate() {
        if (m_pause)
            return;

//        m_onControllerColliderHitAlreadyCalled = false;

		if (launcher == null)
            launcher = GetComponent<AttackLauncher>();
		if (launcher.isAnyBusy())
            return;

		Vector3 direction = transform.forward * m_forwardSpeed + transform.right * m_rightSpeed;
		direction.y = m_yVelocity;
        m_controller.Move(direction * Time.fixedDeltaTime);

        // --------------------------------------------Take a hit --------------------------------------------------
        if (!m_tookAHit)
            return;

        Vector3 speed = m_velocityHit;

        if (m_controller.isGrounded)
        {
            m_yVelocity = 0;
            m_velocityHit.y = 0;
        }
        else
            m_yVelocity -= m_gravity;

        speed += Vector3.up * m_yVelocity;

        if (m_controller.isGrounded && m_speedFrictionHit != 0)
        {
            Vector3 friction = -speed.normalized * m_speedFrictionHit;
            if (Mathf.Sign(speed.x + friction.x) != Mathf.Sign(speed.x))
                speed = Vector3.zero;
            else
            {
                speed += friction;
                m_velocityHit += friction;
            }
        }

        if (speed.magnitude < 1)
            m_tookAHit = false;

        m_controller.Move(speed * Time.fixedDeltaTime);
        // --------------------------------------------End take a hit --------------------------------------------------
	}

	protected abstract void updateInput();

	protected void workFromInput() {
        if (m_pause)
            return;

        if (m_tookAHit)
        {
            if (jump)
                m_tookAHit = false;
            else
                return;
        }

		m_currentMoveSpeed = (!crouch) ? m_runSpeed : m_crouchSpeed;
		m_currentMoveSpeed = (!sprint) ? m_runSpeed : m_sprintSpeed;

		if (m_controller.isGrounded) {
			if (dodge && m_ableToDodge) {
				m_forwardSpeed = 0;
				m_rightSpeed = 0;
				m_dodging = true;
				m_forwardSpeed = input.y * m_dodgeSpeed;
				m_rightSpeed = input.x * m_dodgeSpeed;
			}
			
			if (/*!m_executingAtk1 && */!m_crouched && !m_dodging) {
				m_forwardSpeed = input.y * m_smoothMovement;
				if (m_forwardSpeed > m_currentMoveSpeed) m_forwardSpeed = m_currentMoveSpeed;
				else if (m_forwardSpeed < -m_currentMoveSpeed) m_forwardSpeed = -m_currentMoveSpeed;

				m_rightSpeed = input.x * m_smoothMovement;
				if (m_rightSpeed > m_currentMoveSpeed) m_rightSpeed = m_currentMoveSpeed;
				else if (m_rightSpeed < -m_currentMoveSpeed) m_rightSpeed = -m_currentMoveSpeed;

				if (input.x * m_rightSpeed <= 0) m_rightSpeed = Mathf.Lerp(m_rightSpeed, 0, m_smoothMovement);
				if (input.y * m_forwardSpeed <= 0) m_forwardSpeed = Mathf.Lerp(m_forwardSpeed, 0, m_smoothMovement);

				m_forwardSpeed += m_smoothMovement * input.y;
				m_rightSpeed += m_smoothMovement * input.x;

				Vector2 dir = (new Vector2(m_rightSpeed, m_forwardSpeed)).normalized * m_currentMoveSpeed;
				m_forwardSpeed = dir.y;
				m_rightSpeed = dir.x;
			}

			if (jump) m_yVelocity = (!m_dodging) ? m_jumpSpeed : 0f;
		} else m_yVelocity -= m_gravity;
		
		if (m_dodging) {
			doDodge();
			m_ableToDodge = false;
		}
		
		if (!m_ableToDodge) cdBeforeDodge();
	}

	// ---

	protected void doDodge() {
		if (m_coolDownDodge >= 0)
			m_coolDownDodge -= Time.deltaTime;
		
		if (m_coolDownDodge < 0 && m_controller.isGrounded) {
			m_currentMoveSpeed = m_runSpeed;
			m_coolDownDodge = m_coolDownDodgeTimer;
			m_smoothMovement = m_smoothMovementDodge;
			m_forwardSpeed = 0;
			m_rightSpeed = 0;
			
			m_dodging = false;
		}
	}

	protected void cdBeforeDodge() {
		if (m_cooldownBeforeDodgeTimer >= m_cooldownBeforeDodge) {
			m_ableToDodge = true;
			m_cooldownBeforeDodgeTimer = 0;
		} else m_cooldownBeforeDodgeTimer += Time.deltaTime;
	}

	public GameObject getCurrentGround() {
		RaycastHit hit;
		if (Physics.Raycast(transform.position, -Vector3.up, out hit, GetComponent<Collider>().bounds.extents.y + 0.1f))
			return hit.collider.gameObject;
		else
			return null;
	}

    public void ReceiveMessage(object msg)
    {
        string str = msg as string;
        if (str != null)
        {
            if (str == "Pause")
                m_pause = true;
            else if (str == "UnPause")
                m_pause = false;
        }
    }

    public Vector3 getVelocity()
    {
        //return transform.forward * m_forwardSpeed + transform.right * m_rightSpeed + Vector3.up * m_yVelocity;
        return m_controller.velocity;
    }

    public void setVelocity(Vector3 _velocity)
    {
        m_tookAHit = true;
        m_velocityHit = _velocity;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.collider.gameObject.GetComponent<Terrain>())
        {
            int i = 0;
            i++;
        }
        //         if (!hit.collider.gameObject.GetComponent<Terrain>() && !m_onControllerColliderHitAlreadyCalled)
        //         {
        //             throw new System.Exception("OnControllerColliderHit call from CharacterController");
        //             FlingableRock collidingObject = hit.collider.gameObject.GetComponent<FlingableRock>();
        //             Vector3 vect = collidingObject.getVelocity() - getVelocity();
        // 
        //             Vector3 velocity1Final = (m_mass / (collidingObject.getMass() + m_mass)) * vect;
        //             velocity1Final = velocity1Final.magnitude * hit.normal;
        // 
        //             Vector3 velocity2Final = (-collidingObject.getMass() / (collidingObject.getMass() + m_mass)) * vect;
        //             velocity2Final = velocity2Final.magnitude * -hit.normal;
        // 
        //             collidingObject.setVelocity(velocity1Final);
        //             setVelocity(velocity2Final);
        //         }
    }
    // 
    //     public void setOnControllerColliderHitAlreadyCalled()
    //     {
    //         m_onControllerColliderHitAlreadyCalled = true;
    //     }
}
