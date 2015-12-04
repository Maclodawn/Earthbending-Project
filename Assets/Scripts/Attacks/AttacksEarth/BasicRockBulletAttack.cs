using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicRockBulletAttack : BasicAttack {

	public GameObject rockBullet;

	public float m_OffsetForwardEarth = 1;
    [SerializeField]
    float m_rangeToTakeBullet = 15.0f;

    [SerializeField]
    float m_attack1ForceUp = 32000;
    [SerializeField]
    float m_attack1ForceForward = 1000000;

	// ---

	protected override void updateMe() {
		attack1();
	}

	protected override float WAIT_TIME() {
		return 0.3f;
	}

	// ---

	private void attack1() {
		Collider[] colliders = Physics.OverlapSphere(executer.transform.position, m_rangeToTakeBullet);
		FlingableRock bullet = null;
		
		if (colliders.Length > 0)
		{
			bullet = findBullet(colliders);
			if (!bullet)
				spawnAndFlingBullet("Fire1", m_attack1ForceUp, m_attack1ForceForward);
			else
			{
				//bullet.setUser(executer, false);
				bullet.fling("Fire1", m_attack1ForceUp, m_attack1ForceForward, false);
			}
		}
		else
			spawnAndFlingBullet("Fire1", m_attack1ForceUp, m_attack1ForceForward);
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
			return colliders[closerOne].GetComponent<FlingableRock>();
	}
	
	private void spawnAndFlingBullet(string _buttonToWatch, float _forceUp, float _forceForward) {
		Vector3 spawnProjectile = executer.transform.position + executer.transform.forward * m_OffsetForwardEarth;
		RaycastHit hit;
		if (Physics.Raycast(spawnProjectile, -Vector3.up, out hit, 50))
		{
			if (rockBullet.transform.childCount == 0)
				return;
			
			Transform child = rockBullet.transform.GetChild(0);
			MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
			spawnProjectile = hit.point - new Vector3(0, meshRenderer.bounds.extents.y, 0);
			
			FlingableRock tmpBullet = ((GameObject)Instantiate(rockBullet, spawnProjectile, Quaternion.identity)).GetComponent<FlingableRock>();
			//tmpBullet.setUser(m_username);
			tmpBullet.init(_buttonToWatch, _forceUp, _forceForward);
		}
	}
}
