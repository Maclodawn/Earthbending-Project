using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicAI : MonoBehaviour {

	public GameObject attack1Object, attack2Object, attack3Object;

	private bool attack1Fire, attack2Fire, attack3Fire;

	public float m_OffsetForwardEarth = 1;
	private float m_timerAttack = 0.3f;
	private float m_coolDownAttack = 0.3f;
	public float m_rangeToTakeBullet = 5.0f;
	
	public float m_attack1ForceUp;
	public float m_attack1ForceForward;

	public string m_username = "AIBody";

	// ---

	private Frustum[] frustums;
	private List<GameObject> objects;

	private static float DeltaTime = 0.5f;
	private float timeCount = 0f;

	public void Start() {
		frustums = GetComponentsInChildren<Frustum>();
	}

	public void Update() {
		UpdateFrustum();

		if (timeCount > DeltaTime) {
			timeCount = 0f;

			foreach (GameObject o in objects) {
				if (IsVisible(o) && !IsFriend(o)) {
					attack1();
					break;
				}
			}
		}

		timeCount += Time.deltaTime;
	}

	private void UpdateFrustum() {
		if (frustums == null || frustums.Length == 0) return;

		if (objects != null) objects.Clear();
		else objects = new List<GameObject>();

		foreach (Frustum f in frustums) {
			foreach (GameObject o in f.GetObjects()) {
				if (!objects.Contains(o)) objects.Add(o);
			}
		}
	}

	// be careful, if center node of objects are not visible, it is not visible
	// main interest: detect if another AI is visible or not
	private bool IsVisible(GameObject o) {
		Vector3 origin = transform.position; //better use head position
		Vector3 direction = (o.transform.position - transform.position);

		RaycastHit hit;
		bool touched = Physics.Raycast(origin, direction, out hit);

		if (!touched) return false;

		return hit.collider.gameObject == o;
	}

	private void attack1() {
		Collider[] colliders = Physics.OverlapSphere(transform.position, m_rangeToTakeBullet);
		FlingableRock bullet = null;
		
		if (colliders.Length > 0)
		{
			bullet = findBullet(colliders);
			if (!bullet)
				spawnAndFlingBullet("Fire1", m_attack1ForceUp, m_attack1ForceForward);
			else
			{
				//bullet.setUser(this, false);
				bullet.fling("Fire1", m_attack1ForceUp, m_attack1ForceForward, false);
			}
		}
		else
			spawnAndFlingBullet("Fire1", m_attack1ForceUp, m_attack1ForceForward);
		
		//m_executingAtk1 = true;
	}

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
		Vector3 spawnProjectile = transform.position + transform.forward * m_OffsetForwardEarth;
		RaycastHit hit;
		if (Physics.Raycast(spawnProjectile, -Vector3.up, out hit, 50))
		{
			if (attack1Object.transform.childCount == 0)
				return;

			Transform child = attack1Object.transform.GetChild(0);
			MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
			spawnProjectile = hit.point - new Vector3(0, meshRenderer.bounds.extents.y, 0);
			
			FlingableRock tmpBullet = ((GameObject)Instantiate(attack1Object, spawnProjectile, Quaternion.identity)).GetComponent<FlingableRock>();
			//tmpBullet.setUser(m_username);
			tmpBullet.init(_buttonToWatch, _forceUp, _forceForward);
		}
	}

	private bool IsFriend(GameObject o) {
		return o.GetComponent<BasicAI>() == null && o.GetComponent<CharacterMovement>() == null;
	}
}
