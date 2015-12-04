using UnityEngine;
using System.Collections;

public abstract class CharacterBasic : MonoBehaviour {

	// --- movement parameters

	protected CharacterController m_controller;

	protected float m_currentMoveSpeed = 1.0f;		// vitesse actuelle : mise à jour
	public float m_runSpeed = 7.0f;					// vitesse de course
	public float m_sprintSpeed = 14.0f;				// vitesse de sprint
	public float m_crouchSpeed = 1.0f;				// vitesse accroupi
	public float m_jumpSpeed = 20.0f;				// vitesse quand on saute (utilisée seulement en Y)
	
	public float m_smoothMovement = 0.1f;			// smoothage du mouvement
	protected float m_addSpeed = 0;					// vitesse ajoutée lors de la course avant
	protected float m_gravity = 1.0f;				// gravité : pour quand on saute
	protected float m_yVelocity = 0.0f;				// vélocité en Y par défaut
	protected float m_forwardSpeed = 0;				// vitesse en avant
	protected float m_rightSpeed = 0;				// vitesse sur le côté
	public float m_speedDiagoFactor = 4;			// sqrt(2), division déplacement sur le côté
	
	public bool m_crouched = false;					// pour savoir s'il est accroupi
	
	protected float m_coolDownDodge = 0.15f;		// compteur de temps s'est écoulé depuis le début de l'anim
	public float m_coolDownDodgeTimer = 0.5f;		// combien de temps dure un dodge
	public float m_dodgeSpeed = 30;					// vitesse du dodge
	protected bool m_dodging = false;				// savoir si on est en train de dodger ou pas
	public float m_smoothMovementDodge = 1;			// tweak sur le dodge
	public float m_cooldownBeforeDodge = 1;			// temps à attendre avant de dodge
	protected float m_cooldownBeforeDodgeTimer = 1;	// temps qui s'est écoulé depuis dernier dodge
	protected bool m_ableToDodge = true;			// savoir si on peut dodge

	// --- input management

	//protected bool fwMove, rgMove, lfMove, bkMove;
	protected bool pause;
	protected bool crouched, sprint, jump, dodge;
	protected bool atk1, atk2, atk3;
	protected Vector3 movement;

	// private BasicAttack attack;

	// ---
}
