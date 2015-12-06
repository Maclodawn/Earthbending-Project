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
	
	// ---
	
	public void Start() {
		m_controller = GetComponent<CharacterController>();
		m_currentMoveSpeed = m_runSpeed;
		m_smoothMovementDodge = m_smoothMovement;
	}

	public void Update() {
		updateInput();
		workFromInput();
	}

	public void FixedUpdate() {
		if (launcher == null) launcher = GetComponent<AttackLauncher>();
		if (launcher.isAnyBusy()) return;

		Vector3 direction = transform.forward * m_forwardSpeed + transform.right * m_rightSpeed;
		direction.y = m_yVelocity;
		m_controller.Move(direction * Time.deltaTime);
	}

	protected abstract void updateInput();

	protected void workFromInput() {
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
}
