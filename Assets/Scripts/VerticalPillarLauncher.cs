using UnityEngine;
using System.Collections;

public class VerticalPillarLauncher : MonoBehaviour
{
    public GameObject m_attackObject;

    public string m_username = "";

    public float m_OffsetForwardEarth = 1;

    Collider[] colliderList;

    public float attackCD = 5;
    float attackCDCounter = 0;

    // Update is called once per frame
    void Update()
    {
        if (attackCD == -1)
        {
            basicAttack();
            --attackCD;
        }
        else if (attackCD > 0 && attackCDCounter >= attackCD)
        {
            attackCDCounter = 0;
            basicAttack();
        }
        else
            attackCDCounter += Time.deltaTime;
    }

    void basicAttack()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit = new RaycastHit();
        bool collided = Physics.Raycast(ray, out hit, 5000);

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

        Quaternion rotation = Quaternion.FromToRotation(transform.up, hitGround.normal) * Quaternion.FromToRotation(m_attackObject.transform.forward, transform.forward);
        Vector3 newDirection = rotation * m_attackObject.transform.up;

        float ySize = 0;
        for (int i = 0; i < m_attackObject.transform.childCount; ++i)
        {
            MeshRenderer meshRenderer = m_attackObject.transform.GetChild(i).GetComponent<MeshRenderer>();
            ySize += meshRenderer.bounds.size.y;
        }

        Vector3 vect = newDirection * ySize / 2.0f;
        Instantiate(m_attackObject, hitGround.point - vect, rotation);

        UnityEditor.EditorApplication.isPaused = true;
    }
}
