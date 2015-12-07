using UnityEngine;
using System.Collections;

public class LeanedPillarLauncher : MonoBehaviour
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

        Quaternion xAndzRotation;
        if (transform.up == Vector3.up || transform.up == -Vector3.up)
            xAndzRotation = Quaternion.FromToRotation(transform.up + Vector3.forward * 0.01f, hitGround.normal + Vector3.forward * 0.01f);
        else if (transform.up == Vector3.right || transform.up == -Vector3.right
                 || transform.up == Vector3.forward || transform.up == -Vector3.forward)
            xAndzRotation = Quaternion.FromToRotation(transform.up + Vector3.up * 0.01f, hitGround.normal + Vector3.up * 0.01f);
        else
            xAndzRotation = Quaternion.FromToRotation(transform.up, hitGround.normal);

        Quaternion yRotation;
        if (transform.forward == Vector3.forward || transform.forward == -Vector3.forward)
            yRotation = Quaternion.FromToRotation(m_attackObject.transform.forward + Vector3.right * 0.01f, transform.forward + Vector3.right * 0.01f);
        else if (transform.forward == Vector3.right || transform.forward == -Vector3.right
                 || transform.forward == Vector3.up || transform.forward == -Vector3.up)
            yRotation = Quaternion.FromToRotation(m_attackObject.transform.forward + Vector3.forward * 0.01f, transform.forward + Vector3.forward * 0.01f);
        else
            yRotation = Quaternion.FromToRotation(m_attackObject.transform.forward, transform.forward);

        Quaternion rotation = xAndzRotation * yRotation;

        Instantiate(m_attackObject, hitGround.point, rotation);

        //UnityEditor.EditorApplication.isPaused = true;
    }
}
