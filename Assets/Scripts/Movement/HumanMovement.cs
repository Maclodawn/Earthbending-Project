using UnityEngine;
using System.Collections;

public class HumanMovement : BasicMovement {

	protected override void updateInput() {
        input.x = Input.GetAxisRaw("Horizontal");
		input.y = Input.GetAxisRaw("Vertical");

		sprint = Input.GetButton("Sprint");
		crouch = Input.GetButton("Crouch");
		dodge = Input.GetButtonDown("Dodge");
		jump = Input.GetButtonDown("Jump");
	}
}
