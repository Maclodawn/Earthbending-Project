using UnityEngine;
using System.Collections;

public class CharacterMovement : MonoBehaviour
{
    protected Manager m_manager;
    protected CharacterController m_controller;

    protected float m_currentMoveSpeed = 1.0f;
    public float m_runSpeed = 7.0f;
    public float m_sprintSpeed = 14.0f;
    public float m_crouchSpeed = 1.0f;
    public float m_jumpSpeed = 20.0f;

    public float m_mass = 80;
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
    protected bool m_executingAtk4 = false;
    public GameObject m_attack1Object;
    public GameObject m_attack2Object;
    public GameObject m_attack3Object;
    public GameObject m_attack4Object;

    public string m_username = "";

    private bool m_cursorLocked;

    protected Collider m_collider;

    bool m_tookAHit = false;
    Vector3 m_velocityHit;
    [SerializeField]
    float m_speedFrictionHit;

//    bool m_onControllerColliderHitAlreadyCalled = false;

    // Use this for initialization
    public void init(Manager _manager)
    {
        m_manager = _manager;

        m_controller = GetComponent<CharacterController>();

        m_collider = GetComponent<Collider>();

        m_currentMoveSpeed = m_runSpeed;

        m_smoothMovementDodge = m_smoothMovement;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //FIXME Debug
        //basicAttack2();

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

        if (m_tookAHit)
        {
            if (Input.GetButton("Jump"))
                m_tookAHit = false;
            else
                return;
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
//        m_onControllerColliderHitAlreadyCalled = false;
        // --------------------------------------------Move --------------------------------------------------
        Vector3 direction = transform.forward * m_forwardSpeed + transform.right * m_rightSpeed;
        direction.y = m_yVelocity;
        m_controller.Move(direction * Time.fixedDeltaTime);
        // --------------------------------------------End move --------------------------------------------------

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

    protected virtual void attack()
    {
        if (m_controller.isGrounded && !m_dodging)
        {
            if (Input.GetButtonDown("Fire1"))
                basicAttack1();

            if (Input.GetButtonDown("Fire2"))
            {
                if (Input.GetKey(KeyCode.S))
                    basicAttack4();
                else if (Input.GetKey(KeyCode.Z))
                    basicAttack2();
                else
                    basicAttack3();
            }
        }
    }

    protected virtual void basicAttack1()
    { }

    protected virtual void basicAttack2()
    { }

    protected virtual void basicAttack3()
    { }

    protected virtual void basicAttack4()
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

    void OnCollisionEnter(Collision collision)
    {
        int toto = 0;
        ++toto;
    }

    void OnCollisionStay(Collision collision)
    {
        int toto = 0;
        ++toto;
    }

    void OnCollisionExit(Collision collision)
    {
        int toto = 0;
        ++toto;
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
