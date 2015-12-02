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
		Debug.Log("#objects in frustum: " + objects.Count);
	}

	private void UpdateFrustum() {
		if (frustums == null || frustums.Length == 0) return;

		if (objects != null) objects.Clear();
		else objects = new List<GameObject>();

		foreach (Frustum f in frustums) {
			foreach (GameObject o in f.GetObjects()) {
				objects.Add(o);
			}
		}
	}
}
