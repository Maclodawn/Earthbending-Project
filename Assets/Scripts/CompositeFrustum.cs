using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompositeFrustum : MonoBehaviour {

	private Frustum[] frustums;
	private List<GameObject> objects;

	public void Start() {
		frustums = GetComponentsInChildren<Frustum>();
		objects = new List<GameObject>();
	}

	public List<GameObject> GetObjects() {
		UpdateFrustum();
		return objects;
	}

	private void UpdateFrustum() {
		if (frustums == null || frustums.Length == 0) return;
		
		objects.Clear();
		
		foreach (Frustum f in frustums) {
			foreach (GameObject o in f.GetObjects()) {
				if (!objects.Contains(o) && IsVisible(o)) objects.Add(o);
			}
		}
	}

	private bool IsVisible(GameObject o) {
		Vector3 origin = transform.position; //better use head position
		Vector3 direction = (o.transform.position - transform.position);
		
		RaycastHit hit;
		bool touched = Physics.Raycast(origin, direction, out hit);
		
		if (!touched) return false;
		
		return hit.collider.gameObject == o;
	}

	/*private bool IsFriend(GameObject o) {
		return o.GetComponent<AttackLauncher>() == null && o.GetComponent<CharacterMovement>() == null;
	}*/
}
