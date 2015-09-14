using UnityEngine;
using System.Collections;

public class Player_Movement : MonoBehaviour
{
    public GameObject m_managerObject;
    Manager m_manager;
    CharacterController m_controller;

    public float m_currentMoveSpeed = 1.0f;
    public float m_runSpeed = 7.0f;
    public float m_speedSprinting = 14.0f;
    public float m_speedCrouch = 1.0f;
    public float m_jumpSpeed = 20.0f;

    public float m_smoothMovement = 0.1f;
    float m_addSpeed = 0;
    float m_gravity = 1.0f;
    float m_yVelocity = 0.0f;
    float m_forwardSpeed = 0;
    float m_rightSpeed = 0;
    public float m_speedDiagoFactor = 4;

    public bool m_crouched = false;

    float m_coolDownDodge = 0.15f;
    public float m_coolDownDodgeTimer = 0.5f;
    public float m_speedDodge = 30;
    bool m_dodging = false;
    float m_smoothMovementDodge = 1;
    public float m_cooldownBeforeDodge = 1;
    float m_cooldownBeforeDodgeTimer = 1;
    bool m_ableToDodge = true;

    public float m_OffsetForwardEarth = 1;
    public float m_projOffsetY = -5;
    public float m_projOffsetYearth1 = -1;
    bool m_executingAtk1 = false;
    float m_timerAttack1 = 0.3f;
    float m_coolDownAttack1 = 0.3f;

    bool m_executingAtk2 = false;
    public GameObject m_attack1Object;
    public GameObject m_attack2Object;

    public float m_rangeToTakeBullet = 5.0f;

    public string username = "";

    // Use this for initialization
    void Start()
    {
        m_manager = m_managerObject.GetComponent<Manager>();
        m_controller = GetComponent<CharacterController>();

        m_currentMoveSpeed = m_runSpeed;

        //         if (!GetComponent<NetworkView>().isMine)
        //         {
        //             transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        //             transform.position = new Vector3(0, 0, 0);
        //             GetComponentInChildren<Camera>().enabled = false;
        //             GetComponentInChildren<MouseLook>().enabled = false;
        //             GetComponentInChildren<VisorEarth>().enabled = false;
        //         }
        //        Network.sendRate = 15;
        // 
        //         foreach (GameObject p in (GameObject.FindGameObjectsWithTag("GameController")))
        //         {
        //             username = p.GetComponent<Connection>().myName;
        //         }

        m_smoothMovementDodge = m_smoothMovement;
    }

    // Update is called once per frame
    void Update()
    {
        //         if (GetComponent<NetworkView>().isMine)
        //         {
        if (!GetComponentInChildren<InGameMenu>().m_isEscape)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
            Cursor.visible = true;

        // -------------------------------------Attack1----------------------------------------------
        if (Input.GetButtonDown("Fire1") && m_controller.isGrounded && !m_dodging)
            BasicAttack1();

        if (Input.GetButtonDown("Fire2") && m_controller.isGrounded && !m_dodging)
            BasicAttack2();

        if (m_executingAtk1)
        {
            m_coolDownAttack1 -= Time.deltaTime;
            if (m_coolDownAttack1 <= 0)
            {
                m_coolDownAttack1 = m_timerAttack1;
                m_executingAtk1 = false;
            }
            else
            {
                m_rightSpeed = 0;
                m_forwardSpeed = 0;
            }
        }

        if (m_executingAtk2)
        {
            m_coolDownAttack1 -= Time.deltaTime;
            if (m_coolDownAttack1 <= 0)
            {
                m_coolDownAttack1 = m_timerAttack1;
                m_executingAtk2 = false;
            }
            else
            {
                m_rightSpeed = 0;
                m_forwardSpeed = 0;
            }
        }

        // ------------------------------------------ Crouch ---------------------------------------
        if (Input.GetButton("Crouch"))
            m_currentMoveSpeed = m_speedCrouch;
        if (Input.GetButtonUp("Crouch"))
            m_currentMoveSpeed = m_runSpeed;

        // ------------------------------------------- Sprint -----------------------------------------
        if (Input.GetButton("Sprint"))
            m_currentMoveSpeed = m_speedSprinting;
        if (Input.GetButtonUp("Sprint"))
            m_currentMoveSpeed = m_runSpeed;

        // ------------------------------------------ Dodge ----------------------------------------
        if (Input.GetButtonDown("Dodge") && m_ableToDodge && m_controller.isGrounded)
        {
            m_forwardSpeed = 0;
            m_rightSpeed = 0;
            m_dodging = true;
            if (Input.GetKey(KeyCode.Z))
                m_forwardSpeed = m_speedDodge;
            if (Input.GetKey(KeyCode.S))
                m_forwardSpeed = -m_speedDodge;

            if (Input.GetKey(KeyCode.D))
                m_rightSpeed = m_speedDodge;
            if (Input.GetKey(KeyCode.Q))
                m_rightSpeed = -m_speedDodge;
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
            Dodge();
            m_ableToDodge = false;
        }

        if (!m_ableToDodge)
            CDBeforeDodge();
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

    void BasicAttack1()
    {
        m_manager.m_bulletList.RemoveAll(item => item == null);
        BasicRockBullet bullet = null;

        if (m_manager.m_bulletList.Count > 0)
        {
            bullet = findBullet();
            if (!bullet)
                spawnAndFlingBullet();
            else
            {
                //bullet.setUser(m_id);
                bullet.fling();
            }
        }
        else
            spawnAndFlingBullet();

        m_executingAtk1 = true;
    }

    BasicRockBullet findBullet()
    {
        for (int i = 0; i < m_manager.m_bulletList.Count; ++i)
        {
            if (m_manager.m_bulletList[i].m_user != null)
                continue;

            Vector3 positionA = transform.position;
            Vector3 positionB = m_manager.m_bulletList[i].transform.position;
            float distance = Mathf.Sqrt(Mathf.Pow(positionA.x - positionB.x, 2) + Mathf.Pow(positionA.y - positionB.y, 2) + Mathf.Pow(positionA.z - positionB.z, 2));
            if (distance < m_rangeToTakeBullet)
            {
                return m_manager.m_bulletList[i];
            }
        }
        return null;
    }

    void spawnAndFlingBullet()
    {
       Vector3 spawnProjectile = transform.position + transform.forward * m_OffsetForwardEarth + new Vector3(0, m_projOffsetYearth1, 0);
       BasicRockBullet tmpBullet = ((GameObject)/*Network.*/Instantiate(m_attack1Object, spawnProjectile, Quaternion.identity/*, 0*/)).GetComponent<BasicRockBullet>();
       tmpBullet.m_spawningHeightOffset = m_projOffsetYearth1;
       //tmpBullet.setUser(m_id);
    }

    void BasicAttack2()
    {
//         Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
//         RaycastHit hit;
//         if (Physics.Raycast(ray, out hit, 5000))
//         {
//             if (hit.collider.gameObject.layer == LayerMask.NameToLayer("WallEarth"))
//                 print(hit.collider.gameObject.tag);
//             else
//             {
//                 //Vector3 targetDir = hit.point - (transform.position + transform.forward * OffsetForwardEarth + new Vector3(0, projOffsetYearth1, 0));
//                 //float step = 10 * Time.deltaTime;
//                 //Vector3 newDir = Vector3.RotateTowards(transform.forward + new Vector3(0, projOffsetYearth1, 0), targetDir, step, 0.0f);
// //                Network.Instantiate(m_attack2Object, hit.point + new Vector3(0, -3, 0), transform.rotation, 0);
//                 m_executingAtk2 = true;
//                 print(hit.collider.gameObject.name);
//             }
//         }
    }

    void Dodge()
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

    void CDBeforeDodge()
    {
        if (m_cooldownBeforeDodgeTimer >= m_cooldownBeforeDodge)
        {
            m_ableToDodge = true;
            m_cooldownBeforeDodgeTimer = 0;
        }
        else
            m_cooldownBeforeDodgeTimer += Time.deltaTime;
    }

//     void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
//     {
//         if (stream.isWriting)
//         {
//             positionSend = transform.position;
//             RotationSend = transform.rotation;
//             stream.Serialize(ref positionSend);
//             stream.Serialize(ref RotationSend);
//         }
//         else
//         {
//             stream.Serialize(ref positionSend);
//             stream.Serialize(ref RotationSend);
//             positionReceive = positionSend;
//             RotationReceive = RotationSend;
//         }
//     }
}
