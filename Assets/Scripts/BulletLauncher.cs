using UnityEngine;
using System.Collections;

public class BulletLauncher : MonoBehaviour
{
    public GameObject m_attackObject;

    public string m_username = "";

    public float m_OffsetForwardEarth = 1;

    Collider[] colliderList;

    [SerializeField]
    float m_attack1ForceUp = 32000;
    [SerializeField]
    float m_attack1ForceForward = 1000000;

    AttackLauncher m_launcher;

    public float attackCD = 5;
    float attackCDCounter = 0;

    // Update is called once per frame
    void Update()
    {
        if (attackCDCounter >= attackCD)
        {
            attackCDCounter = 0;
            basicAttack1();
        }
        else
            attackCDCounter += Time.deltaTime;
    }

    void basicAttack1()
    {
        //spawnAndFlingBullet("Cheat", m_attack1ForceUp, m_attack1ForceForward);
    }

    void spawnAndFlingBullet(AttackLauncher _launcher, float _forceUp, float _forceForward)
    {
        Vector3 spawnProjectile = transform.position + transform.forward * m_OffsetForwardEarth;
        RaycastHit hit;
        if (Physics.Raycast(spawnProjectile, -Vector3.up, out hit, 50))
        {
            if (m_attackObject.transform.childCount == 0)
                return;
            Transform child = m_attackObject.transform.GetChild(0);
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            spawnProjectile = hit.point - new Vector3(0, meshRenderer.bounds.extents.y, 0);

            FlingableRock tmpBullet = ((GameObject)Instantiate(m_attackObject, spawnProjectile, Quaternion.identity)).GetComponent<FlingableRock>();
            tmpBullet.setUser(gameObject);
            tmpBullet.init(_launcher, _forceUp, _forceForward);
        }
    }
}
