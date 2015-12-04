using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
	public Manager ok;

    protected Manager m_manager;
    protected CharacterController m_controller;

    protected float m_currentMoveSpeed = 1.0f;
    public float m_runSpeed = 7.0f;
    public float m_sprintSpeed = 14.0f;
    public float m_crouchSpeed = 1.0f;
    public float m_jumpSpeed = 20.0f;

    public float m_smoothMovement = 0.1f;
    protected float m_addSpeed = 0;
    protected float m_gravity = 1.0f;
    protected float m_yVelocity = 0.0f;
    protected float m_forwardSpeed = 0;
    protected float m_rightSpeed = 0;
    public float m_speedDiagoFactor = 4;

    public bool m_crouched = false;

    protected float m_coolDownDodge = 0.15f;
    public float m_coolDownDodgeTimer = 0.5f;
    public float m_dodgeSpeed = 30;
    protected bool m_dodging = false;
    public float m_smoothMovementDodge = 1;
    public float m_cooldownBeforeDodge = 1;
    protected float m_cooldownBeforeDodgeTimer = 1;
    protected bool m_ableToDodge = true;

    protected bool m_executingAtk1 = false;
    protected bool m_executingAtk2 = false;
    protected bool m_executingAtk3 = false;
    public GameObject m_attack1Object;
    public GameObject m_attack2Object;
    public GameObject m_attack3Object;

    public string m_username = "";

    private bool m_cursorLocked;

    // Use this for initialization
    public void init(Manager _manager)
    {
        m_manager = _manager;

        m_controller = GetComponent<CharacterController>();

        m_currentMoveSpeed = m_runSpeed;

        m_smoothMovementDodge = m_smoothMovement;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!GetComponentInChildren<InGameMenu>().m_isEscape)
        {
            if (!m_cursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                m_cursorLocked = true;
            }
        }
        else if (m_cursorLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // -------------------------------------Attack----------------------------------------------
        attack();

        // ------------------------------------------ Crouch ---------------------------------------
        if (Input.GetButton("Crouch"))
            m_currentMoveSpeed = m_crouchSpeed;
        if (Input.GetButtonUp("Crouch"))
            m_currentMoveSpeed = m_runSpeed;

        // ------------------------------------------- Sprint -----------------------------------------
        if (Input.GetButton("Sprint"))
            m_currentMoveSpeed = m_sprintSpeed;
        if (Input.GetButtonUp("Sprint"))
            m_currentMoveSpeed = m_runSpeed;

        // ------------------------------------------ Dodge ----------------------------------------
        if (Input.GetButtonDown("Dodge") && m_ableToDodge && m_controller.isGrounded)
        {
            m_forwardSpeed = 0;
            m_rightSpeed = 0;
            m_dodging = true;
            if (Input.GetKey(KeyCode.Z))
                m_forwardSpeed = m_dodgeSpeed;
            if (Input.GetKey(KeyCode.S))
                m_forwardSpeed = -m_dodgeSpeed;

            if (Input.GetKey(KeyCode.D))
                m_rightSpeed = m_dodgeSpeed;
            if (Input.GetKey(KeyCode.Q))
                m_rightSpeed = -m_dodgeSpeed;
        }

        // ------------------------------------------- Movement -----------------------------------------
        if (!m_executingAtk1 && !m_crouched && !m_dodging)
        {
            if (m_controller.isGrounded)
            {
                if (Input.GetKey(KeyCode.Z))
                {
                    m_forwardSpeed += m_smoothMovement + m_addSpeed;
                    if (m_forwardSpeed > m_currentMoveSpeed)
                        m_forwardSpeed = m_currentMoveSpeed;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    m_forwardSpeed -= m_smoothMovement;
                    if (m_forwardSpeed < -m_currentMoveSpeed)
                        m_forwardSpeed = -m_currentMoveSpeed;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    m_rightSpeed += m_smoothMovement;
                    if (m_rightSpeed > m_currentMoveSpeed)
                        m_rightSpeed = m_currentMoveSpeed;
                }

                if (Input.GetKey(KeyCode.Q))
                {
                    m_rightSpeed -= m_smoothMovement;
                    if (m_rightSpeed < -m_currentMoveSpeed)
                        m_rightSpeed = -m_currentMoveSpeed;
                }
            }

            if (m_controller.isGrounded)
            {
                if (!(Input.GetKey(KeyCode.D)) && (m_rightSpeed > 0))
                    m_rightSpeed = Mathf.Lerp(m_rightSpeed, 0, m_smoothMovement);

                if (!(Input.GetKey(KeyCode.Q)) && (m_rightSpeed < 0))
                    m_rightSpeed = Mathf.Lerp(m_rightSpeed, 0, m_smoothMovement);

                if (!(Input.GetKey(KeyCode.S)) && (m_forwardSpeed < 0))
                    m_forwardSpeed = Mathf.Lerp(m_forwardSpeed, 0, m_smoothMovement);

                if (!(Input.GetKey(KeyCode.Z)) && (m_forwardSpeed > 0))
                    m_forwardSpeed = Mathf.Lerp(m_forwardSpeed, 0, m_smoothMovement);
            }

            if (m_controller.isGrounded)
            {
                if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.D))
                {
                    m_forwardSpeed += m_smoothMovement;
                    m_rightSpeed += m_smoothMovement;

                    if (m_forwardSpeed > m_currentMoveSpeed / m_speedDiagoFactor)
                        m_forwardSpeed = m_currentMoveSpeed / m_speedDiagoFactor;

                    if (m_rightSpeed > m_currentMoveSpeed / m_speedDiagoFactor)
                        m_rightSpeed = m_currentMoveSpeed / m_speedDiagoFactor;
                }

                if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.Q))
                {
                    m_forwardSpeed += m_smoothMovement;
                    m_rightSpeed -= m_smoothMovement;

                    if (m_forwardSpeed > m_currentMoveSpeed / m_speedDiagoFactor)
                        m_forwardSpeed = m_currentMoveSpeed / m_speedDiagoFactor;

                    if (m_rightSpeed < -m_currentMoveSpeed / m_speedDiagoFactor)
                        m_rightSpeed = -m_currentMoveSpeed / m_speedDiagoFactor;
                }

                if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
                {
                    m_forwardSpeed -= m_smoothMovement;
                    m_rightSpeed += m_smoothMovement;

                    if (m_forwardSpeed < -m_currentMoveSpeed / m_speedDiagoFactor)
                        m_forwardSpeed = -m_currentMoveSpeed / m_speedDiagoFactor;

                    if (m_rightSpeed > m_currentMoveSpeed / m_speedDiagoFactor)
                        m_rightSpeed = m_currentMoveSpeed / m_speedDiagoFactor;
                }

                if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Q))
                {
                    m_forwardSpeed -= m_smoothMovement;
                    m_rightSpeed -= m_smoothMovement;

                    if (m_forwardSpeed < -m_currentMoveSpeed / m_speedDiagoFactor)
                        m_forwardSpeed = -m_currentMoveSpeed / m_speedDiagoFactor;

                    if (m_rightSpeed < -m_currentMoveSpeed / m_speedDiagoFactor)
                        m_rightSpeed = -m_currentMoveSpeed / m_speedDiagoFactor;
                }
            }
        }
        // ------------------------------------------- End Movement -----------------------------------------

        // --------------------------------------------Jump --------------------------------------------------
        if (m_controller.isGrounded)
        {
            if (Input.GetButtonDown("Jump") && !m_dodging)
                m_yVelocity = m_jumpSpeed;
            else if (Input.GetButtonDown("Jump") && m_dodging)
                m_yVelocity = 0;
        }
        else
            m_yVelocity -= m_gravity;

        if (m_dodging)
        {
            dodge();
            m_ableToDodge = false;
        }

        if (!m_ableToDodge)
            cdBeforeDodge();
        // --------------------------------------------End Jump --------------------------------------------------
    }

    void FixedUpdate()
    {
        // --------------------------------------------Move --------------------------------------------------
        Vector3 direction = transform.forward * m_forwardSpeed + transform.right * m_rightSpeed;
        direction.y = m_yVelocity;
        m_controller.Move(direction * Time.deltaTime);
        // --------------------------------------------End move --------------------------------------------------
    }

    protected virtual void attack()
    {
        if (Input.GetButtonDown("Fire1") && m_controller.isGrounded && !m_dodging)
            basicAttack1();

        if (Input.GetButtonDown("Fire2") && m_controller.isGrounded && !m_dodging)
            basicAttack2();

        if (Input.GetButtonDown("Fire3") && m_controller.isGrounded && !m_dodging)
            basicAttack3();
    }

    protected virtual void basicAttack1()
    { }

    protected virtual void basicAttack2()
    { }

    protected virtual void basicAttack3()
    { }

    void dodge()
    {
        if (m_coolDownDodge >= 0)
            m_coolDownDodge -= Time.deltaTime;

        if (m_coolDownDodge < 0 && m_controller.isGrounded)
        {
            m_currentMoveSpeed = m_runSpeed;
            m_coolDownDodge = m_coolDownDodgeTimer;
            m_smoothMovement = m_smoothMovementDodge;
            m_forwardSpeed = 0;
            m_rightSpeed = 0;

            m_dodging = false;
        }
    }

    void cdBeforeDodge()
    {
        if (m_cooldownBeforeDodgeTimer >= m_cooldownBeforeDodge)
        {
            m_ableToDodge = true;
            m_cooldownBeforeDodgeTimer = 0;
        }
        else
            m_cooldownBeforeDodgeTimer += Time.deltaTime;
    }
}
