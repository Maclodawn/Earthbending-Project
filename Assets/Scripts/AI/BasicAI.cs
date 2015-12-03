using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicAI : MonoBehaviour {

	private Frustum[] frustums;
	private List<GameObject> objects;

	public void Start() {
		frustums = GetComponentsInChildren<Frustum>();
	}

	public void Update() {
		UpdateFrustum();

		int nbVisible = 0;
		foreach (GameObject o in objects) {
			if (IsVisible(o)) ++nbVisible;
		}

		Debug.Log("#objects in frustum: " + objects.Count + " visible: " + nbVisible);

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
		Physics.Raycast(origin + direction.normalized/2, direction, out hit);

		return hit.collider.gameObject == o;
	}
}
