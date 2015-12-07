using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RockBulletAttack : EarthAttack {

	public GameObject rockBullet;
	public FlingableRock myCurrentBullet;
	
    [SerializeField]
    float m_rangeToTakeBullet = 15.0f;
	
	// ---

	protected override void updateMe() {
		basicAttack1();
	}

	protected override float WAIT_TIME() {
		return 0.3f;
	}

	public override bool isFinished() {
		return myCurrentBullet == null ||
			  (myCurrentBullet != null
			  && myCurrentBullet.transform.position.y >= myCurrentBullet.getHeightToReach());
	}

	private void basicAttack1()
    {
        AttackLauncher atkLauncher = GetComponent<AttackLauncher>();

        Ray ray = atkLauncher.getAimRay();
		RaycastHit hit;
		bool collided = Physics.Raycast(ray, out hit, 5000);

        BreakableRock breakableRock = null;
        if (hit.collider)
            breakableRock = hit.collider.GetComponentInParent<BreakableRock>();

        if (Physics.Raycast(ray, out hit, 5000))
        {
            if (collided && breakableRock != null)
            {
                breakableRock.breakRock(gameObject, GetComponent<AttackLauncher>(), m_attack1ForceUp, m_attack1ForceForward);
            }
            else
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, m_rangeToTakeBullet);
                FlingableRock bullet = null;

                if (colliders.Length > 0)
                {
                    bullet = findBullet(colliders);
                    if (!bullet)
                        spawnAndFlingBullet(GetComponent<AttackLauncher>(), m_attack1ForceUp, m_attack1ForceForward);
                    else
                    {
                        //                 GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        //                 sphere.transform.position = transform.position;
                        //                 sphere.transform.localScale = new Vector3(1, 1, 1) * m_rangeToTakeBullet;
                        //                 sphere.GetComponent<SphereCollider>().enabled = false
                        //                UnityEditor.EditorApplication.isPaused = true;

                        bullet.setUser(gameObject);
                        myCurrentBullet = bullet;
                        bullet.fling(GetComponent<AttackLauncher>(), m_attack1ForceUp, m_attack1ForceForward, false);
                    }
                }
                else
                    spawnAndFlingBullet(GetComponent<AttackLauncher>(), m_attack1ForceUp, m_attack1ForceForward);
            }
        }
	}

	// ---
	
	private FlingableRock findBullet(Collider[] colliders) {
		int closerOne = -1;
		float closerDist = 0;
		
		for (int i = 0; i < colliders.Length; ++i)
		{
			FlingableRock rock = colliders[i].GetComponent<FlingableRock>();
			
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
            FlingableRock flingableRock = colliders[closerOne].GetComponent<FlingableRock>();

            if (flingableRock.canRiseInMinTime(0.30f, gameObject, m_attack1ForceUp))
                return flingableRock;

            return null;
        }
	}
	
	private void spawnAndFlingBullet(AttackLauncher _launcher, float _forceUp, float _forceForward) {
		Vector3 spawnProjectile = transform.position + transform.forward * m_OffsetForwardEarth;
		RaycastHit hit;
		if (Physics.Raycast(spawnProjectile, -Vector3.up, out hit, 50))
		{
			if (rockBullet.transform.childCount == 0)
				return;
			
			Transform child = rockBullet.transform.GetChild(0);
			MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
			spawnProjectile = hit.point - new Vector3(0, meshRenderer.bounds.extents.y, 0);
			
			FlingableRock tmpBullet = ((GameObject)Instantiate(rockBullet, spawnProjectile, Quaternion.identity)).GetComponent<FlingableRock>();
			tmpBullet.setUser(gameObject);
			myCurrentBullet = tmpBullet;
			tmpBullet.init(_launcher, _forceUp, _forceForward);
		}
	}
}
