using UnityEngine;
using System.Collections;

public class CharacterMovementEarth : CharacterMovement
{
    public float m_OffsetForwardEarth = 1;
    public float m_projOffsetYEarth1 = -1;
    float m_timerAttack1 = 0.3f;
    float m_coolDownAttack1 = 0.3f;
    public float m_rangeToTakeBullet = 5.0f;

    Collider[] colliderList;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void attack()
    {
        base.attack();

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
    }

    protected override void basicAttack1()
    {
        colliderList = Physics.OverlapSphere(transform.position, m_rangeToTakeBullet);
        BasicRockBullet bullet = null;

        if (colliderList.Length > 0)
        {
            bullet = findBullet();
            if (!bullet)
                spawnAndFlingBullet();
            else
            {
                bullet.setUser(m_username);
                bullet.fling();
            }
        }
        else
            spawnAndFlingBullet();

        m_executingAtk1 = true;
    }

    BasicRockBullet findBullet()
    {
        for (int i = 0; i < colliderList.Length; ++i)
        {
            BasicRockBullet rock = colliderList[i].GetComponent<BasicRockBullet>();

            if (rock != null)
            {
                if (rock.m_user != null)
                    continue;

                Vector3 positionA = transform.position;
                Vector3 positionB = rock.transform.position;
                float distance = Vector3.Distance(positionA, positionB);
                if (distance < m_rangeToTakeBullet)
                {
                    return rock;
                }
            }
        }
        return null;
    }

    void spawnAndFlingBullet()
    {
        Vector3 spawnProjectile = transform.position + transform.forward * m_OffsetForwardEarth + new Vector3(0, m_projOffsetYEarth1, 0);
        BasicRockBullet tmpBullet = ((GameObject)Instantiate(m_attack1Object, spawnProjectile, Quaternion.identity)).GetComponent<BasicRockBullet>();
        tmpBullet.init();
        tmpBullet.m_spawningHeightOffset = m_projOffsetYEarth1;
        tmpBullet.setUser(m_username);
    }

    protected override void basicAttack2()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5000))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("WallEarth"))
                print(hit.collider.gameObject.name);
            else
            {
                Quaternion rotation = Quaternion.FromToRotation(transform.up, hit.normal) * Quaternion.FromToRotation(m_attack2Object.transform.forward, transform.forward);
                Vector3 newDirection = rotation * m_attack2Object.transform.up;

                float ySize = 0;
                for (int i = 0; i < m_attack2Object.transform.childCount; ++i)
                {
                    MeshRenderer meshRenderer = m_attack2Object.transform.GetChild(i).GetComponent<MeshRenderer>();
                    ySize += meshRenderer.bounds.size.y;
                }

                Vector3 vect = newDirection * ySize / 2.0f;
                Instantiate(m_attack2Object, hit.point - vect, rotation);
                m_executingAtk2 = true;
            }
        }
    }
}