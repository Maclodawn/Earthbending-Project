using UnityEngine;
using System.Collections;

public class CharacterMovementEarth : CharacterMovement
{
    public float m_OffsetForwardEarth = 1;
    float m_timerAttack = 0.3f;
    float m_coolDownAttack = 0.3f;
    [SerializeField]
    float m_rangeToTakeBullet = 5.0f;

    Collider[] colliderList;

    [SerializeField]
    float m_attack1ForceUp;
    [SerializeField]
    float m_attack1ForceForward;

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
            m_coolDownAttack -= Time.deltaTime;
            if (m_coolDownAttack <= 0)
            {
                m_coolDownAttack = m_timerAttack;
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
            m_coolDownAttack -= Time.deltaTime;
            if (m_coolDownAttack <= 0)
            {
                m_coolDownAttack = m_timerAttack;
                m_executingAtk2 = false;
            }
            else
            {
                m_rightSpeed = 0;
                m_forwardSpeed = 0;
            }
        }

        if (m_executingAtk3)
        {
            m_coolDownAttack -= Time.deltaTime;
            if (m_coolDownAttack <= 0)
            {
                m_coolDownAttack = m_timerAttack;
                m_executingAtk3 = false;
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
        FlingableRock bullet = null;

        if (colliderList.Length > 0)
        {
            bullet = findBullet();
            if (!bullet)
                spawnAndFlingBullet("Fire1", m_attack1ForceUp, m_attack1ForceForward);
            else
            {
                //bullet.setUser(m_username);
                bullet.fling("Fire1", m_attack1ForceUp, m_attack1ForceForward, false);
            }
        }
        else
            spawnAndFlingBullet("Fire1", m_attack1ForceUp, m_attack1ForceForward);

        m_executingAtk1 = true;
    }

    FlingableRock findBullet()
    {
        int closerOne = -1;
        float closerDist = 0;

        for (int i = 0; i < colliderList.Length; ++i)
        {
            FlingableRock rock = colliderList[i].GetComponent<FlingableRock>();

            if (rock != null)
            {
                if (rock.m_user != null)
                    continue;

                float distance = Vector3.Distance(transform.position, rock.transform.position);
                if (distance < m_rangeToTakeBullet && (closerOne == -1 || closerDist > distance))
                {
                    closerDist = distance;
                    closerOne = i;
                }
            }
        }

        if (closerOne == -1)
            return null;
        else
        {
            FlingableRock flingableRock = colliderList[closerOne].GetComponent<FlingableRock>();

            if (flingableRock.canRiseInMinTime(0.30f, this))
                return flingableRock;

            return null;
        }
    }

    void spawnAndFlingBullet(string _buttonToWatch, float _forceUp, float _forceForward)
    {
        Vector3 spawnProjectile = transform.position + transform.forward * m_OffsetForwardEarth;
        RaycastHit hit;
        if (Physics.Raycast(spawnProjectile, -Vector3.up, out hit, 50))
        {
            if (m_attack1Object.transform.childCount == 0)
                return;
            Transform child = m_attack1Object.transform.GetChild(0);
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            spawnProjectile = hit.point - new Vector3(0, meshRenderer.bounds.extents.y, 0);

            FlingableRock tmpBullet = ((GameObject)Instantiate(m_attack1Object, spawnProjectile, Quaternion.identity)).GetComponent<FlingableRock>();
            //tmpBullet.setUser(m_username);
            tmpBullet.init(_buttonToWatch, _forceUp, _forceForward);
        }
    }

    protected override void basicAttack2()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
        RaycastHit hit = new RaycastHit();
        bool collided = Physics.Raycast(ray, out hit, 5000);

        BreakableRock breakableRock = hit.collider.GetComponentInParent<BreakableRock>();

        if (collided && breakableRock != null)
        {
            breakableRock.breakRock(m_username, "Fire2", m_attack1ForceUp, m_attack1ForceForward);
        }
        else
        {
            if (!collided)
                hit.point = ray.direction * 5000;

            Vector3 direction = hit.point - transform.position;
            direction.Normalize();

            RaycastHit hitGround;
            {
                Vector3 origin = transform.position + transform.forward * m_OffsetForwardEarth * 4;
                if (!Physics.Raycast(origin, -Vector3.up, out hitGround, 50))
                    Physics.Raycast(origin, Vector3.up, out hitGround, 50);
            }

            if (!hitGround.collider.gameObject.name.Contains("Terrain"))
                return;

            Quaternion rotation = Quaternion.FromToRotation(transform.up, hitGround.normal) * Quaternion.FromToRotation(m_attack2Object.transform.forward, transform.forward);

            Instantiate(m_attack2Object, hitGround.point, rotation);
        }
    }

    protected override void basicAttack3()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5000))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("VerticalPillarEarth"))
            {
                BreakableRockWall breakableRockWall = hit.collider.gameObject.GetComponent<BreakableRockWall>();
                breakableRockWall.breakRock(m_username, "Fire2", m_attack1ForceUp, m_attack1ForceForward);
            }
            else
            {
                Quaternion rotation = Quaternion.FromToRotation(transform.up, hit.normal) * Quaternion.FromToRotation(m_attack3Object.transform.forward, transform.forward);
                Vector3 newDirection = rotation * m_attack3Object.transform.up;

                float ySize = 0;
                for (int i = 0; i < m_attack3Object.transform.childCount; ++i)
                {
                    MeshRenderer meshRenderer = m_attack3Object.transform.GetChild(i).GetComponent<MeshRenderer>();
                    ySize += meshRenderer.bounds.size.y;
                }

                Vector3 vect = newDirection * ySize / 2.0f;
                Instantiate(m_attack3Object, hit.point - vect, rotation);
                m_executingAtk3 = true;
            }
        }
    }

    protected override void basicAttack4()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 5000))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("WallEarth"))
            {
                BreakableRockWall breakableRockWall = hit.collider.gameObject.GetComponent<BreakableRockWall>();
                breakableRockWall.breakRock(m_username, "Fire2", m_attack1ForceUp, m_attack1ForceForward);
            }
            else
            {
                Quaternion rotation = Quaternion.FromToRotation(transform.up, hit.normal) * Quaternion.FromToRotation(m_attack4Object.transform.forward, transform.forward);
                Vector3 newDirection = rotation * m_attack4Object.transform.up;

                float ySize = 0;
                for (int i = 0; i < m_attack4Object.transform.childCount; ++i)
                {
                    MeshRenderer meshRenderer = m_attack4Object.transform.GetChild(i).GetComponent<MeshRenderer>();
                    ySize += meshRenderer.bounds.size.y;
                }

                Vector3 vect = newDirection * ySize / 2.0f;
                Instantiate(m_attack4Object, hit.point - vect, rotation);
                m_executingAtk4 = true;
            }
        }
    }

    public GameObject getCurrentGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, m_collider.bounds.extents.y + 0.1f))
            return hit.collider.gameObject;
        else
            return null;
    }
}