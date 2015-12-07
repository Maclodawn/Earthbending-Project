using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BreakableRockPillar : BreakableRock
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        Physics.IgnoreCollision(m_boxCollider, Manager.getManager().m_terrain.GetComponent<Collider>());

        float angle = m_boxCollider.transform.eulerAngles.z * Mathf.PI / 180;
        float adjacent = transform.forward.magnitude;
        float hypothenus = adjacent / Mathf.Cos(angle);
        float opposite = hypothenus * Mathf.Sin(angle);
        Vector3 normal = Vector3.Cross(transform.forward, transform.right).normalized;

        m_forwardOut = (transform.forward + normal * opposite);

        moveTopAtPosition();

//         m_previousPos[0] = transform.position;
//         m_previousPos[1] = transform.position;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void updateNotFrozenYet()
    {
        Ray ray = new Ray(transform.position - transform.up * (m_size.y * transform.localScale.x / 2 - m_baseSize.y), -transform.up);
        RaycastHit hit;
        bool rayCast = Physics.Raycast(ray, out hit, 10);

        if (m_exitObstructed || rayCast && hit.transform.gameObject.name.Contains("Terrain"))
        {
            //m_rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            m_rigidBody.isKinematic = true;
            m_notFrozen = false;
        }
    }

    protected override void scaleIt(GameObject _gameObject)
    {
        //FIXME : Besoin d'avoir des mesh dont le scale est à 1 même si leur taille ne fait pas 1
        _gameObject.transform.localScale = transform.localScale;
    }

    protected override void moveTopAtPosition()
    {
        float y = m_size.y / 4 - m_baseSize.y;
        float x = m_size.x / 4;
        float z = m_size.z / 4;
        float xz = Mathf.Sqrt(x * x + z * z);
        transform.position -= m_forwardOut * Mathf.Sqrt(y * y + xz * xz);
    }

    public override void setVelocity(Vector3 _velocity)
    {
        if (_velocity.x < 0 || _velocity.y < 0 || _velocity.z < 0)
        {
            m_rigidBody.velocity = Vector3.zero;
            m_exitObstructed = true;
        }
        else
            m_rigidBody.velocity = _velocity;
    }
}
