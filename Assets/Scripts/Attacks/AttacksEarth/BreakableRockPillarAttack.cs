using UnityEngine;
using System.Collections;

public class BreakableRockPillarAttack : EarthAttack {

	public GameObject rockPillar;

	protected override void updateMe() {
		basicAttack2();
	}
	
	protected override float WAIT_TIME() {
		return 0.3f;
	}

    protected void basicAttack2()
    {
        Ray ray = GetComponent<AttackLauncher>().getAimRay();
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

        //             Debug.DrawRay(hitGround.point, transform.up, Color.blue);
        //             Debug.DrawRay(hitGround.point, hitGround.normal, Color.cyan);
        //             Debug.DrawRay(hitGround.point, rockPillar.transform.forward, Color.red);
        //             Debug.DrawRay(hitGround.point, transform.forward, Color.magenta);
        //             UnityEditor.EditorApplication.isPaused = true;

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
            yRotation = Quaternion.FromToRotation(rockPillar.transform.forward + Vector3.right * 0.01f, transform.forward + Vector3.right * 0.01f);
        else if (transform.forward == Vector3.right || transform.forward == -Vector3.right
                 || transform.forward == Vector3.up || transform.forward == -Vector3.up)
            yRotation = Quaternion.FromToRotation(rockPillar.transform.forward + Vector3.forward * 0.01f, transform.forward + Vector3.forward * 0.01f);
        else
            yRotation = Quaternion.FromToRotation(rockPillar.transform.forward, transform.forward);

        Quaternion rotation = xAndzRotation * yRotation;

        Instantiate(rockPillar, hitGround.point, rotation);
        GetComponent<BasicMovement>().m_Animator.Play("Attack 02");
        GetComponent<BasicMovement>().m_Animator.CrossFade("Grounded", 1f);
    }
}
