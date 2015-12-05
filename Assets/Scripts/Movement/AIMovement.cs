using UnityEngine;
using System.Collections;

public class AIMovement : BasicMovement {

	protected override void updateInput() {
		float degrees = Random.Range(-15f, 15f);
		transform.Rotate(0f, degrees, 0f);
		input.x = -Mathf.Sin(degrees*Mathf.Deg2Rad);
		input.y = Mathf.Cos(degrees*Mathf.Deg2Rad);

		//Debug.DrawLine(transform.position, transform.position + transform.forward);

		jump = (Mathf.Abs(degrees) > 59.5f) ? true : false;
		sprint = (Mathf.Abs(degrees) > 55 && Mathf.Abs(degrees) < 57) ? true : false;
	}
}
