using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Frustum : MonoBehaviour {

	public List<GameObject> objects;

	public void OnTriggerExit(Collider collider) {
		objects.Remove(collider.gameObject);
	}

	public void OnTriggerEnter(Collider collider) {
		objects.Add(collider.gameObject);
	}

	public List<GameObject> GetObjects() {
		return objects;
	}
}
