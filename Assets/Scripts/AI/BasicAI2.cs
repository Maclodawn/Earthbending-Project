using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicAI2 : CharacterBasic {
	
	private BasicAttack attack1;
	
	private Frustum[] frustums;
	private List<GameObject> objects;

	// ---

	private static float DeltaTime = 0.5f;
	private float timeCount = 0f;

	// ---
	
	public void Start() {
		frustums = GetComponentsInChildren<Frustum>();
		objects = new List<GameObject>();
		attack1 = GetComponent<BasicRockBulletAttack>();
	}
	
	public void Update() {
		UpdateFrustum();
		
		if (timeCount > DeltaTime) {
			timeCount = 0f;
			
			if (objects.Count > 0 && !IsFriend(objects[0]))
				attack1.executeAttack(); //TODO: give target position
		}
		
		timeCount += Time.deltaTime;
	}

	// ---
	
	private void UpdateFrustum() {
		if (frustums == null || frustums.Length == 0) return;
		
		objects.Clear();
		
		foreach (Frustum f in frustums) {
			foreach (GameObject o in f.GetObjects()) {
				if (!objects.Contains(o) && IsVisible(o)) objects.Add(o);
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

	//TODO do something...
	private bool IsFriend(GameObject o) {
		return o.GetComponent<BasicAI>() == null && o.GetComponent<CharacterMovement>() == null;
	}
}
