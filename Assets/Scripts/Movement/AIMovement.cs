using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIMovement : BasicMovement {

	private CompositeFrustum frustum;

	protected override void updateInput() {
		//TODO: AI thinking
		if (frustum == null) frustum = GetComponent<CompositeFrustum>();

		List<GameObject> visibles = frustum.GetObjects();
		GameObject nearest = null;
		float distToNearest = 999f;

		foreach (GameObject o in visibles) {
			float distToO = Vector3.Distance(o.transform.position, transform.position);
			if (nearest == null ||  distToNearest > distToO) {
				distToNearest = distToO;
				nearest = o;
			}
		}

		if (nearest != null) {
			//seek

			Vector3 direction = (nearest.transform.position - transform.position);
			direction.y = 0;
			direction.Normalize();
			direction = Quaternion.FromToRotation(direction, Vector3.right) * direction;

			input = new Vector2(input.y, input.x);

			jump = false;
			sprint = Random.Range(0, 10) > 7 ? true : false;
		} else {
			//wander&random walking

			float degrees = Random.Range(-15f, 15f);
			transform.Rotate(0f, degrees, 0f);
			input.x = -Mathf.Sin(degrees*Mathf.Deg2Rad);
			input.y = Mathf.Cos(degrees*Mathf.Deg2Rad);

			//Debug.DrawLine(transform.position, transform.position + transform.forward);

			jump = (Mathf.Abs(degrees) > 59.5f) ? true : false;
			sprint = (Mathf.Abs(degrees) > 55 && Mathf.Abs(degrees) < 57) ? true : false;
		}
	}
}
