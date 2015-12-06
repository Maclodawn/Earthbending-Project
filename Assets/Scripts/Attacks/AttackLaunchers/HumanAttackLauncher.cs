using UnityEngine;
using System.Collections;

public class HumanAttackLauncher : AttackLauncher {

	protected override void updateInput() {
		if (Input.GetButtonDown("Fire1")) {
			atk = 0;
			hold = true;
		}

		if (Input.GetButtonUp("Fire1"))
			hold = false;
		
		if (Input.GetButtonDown("Fire2")) {
			if (Input.GetKey(KeyCode.S))
				atk = 4;
			else if (Input.GetKey(KeyCode.Z))
				atk = 2;
			else
				atk = 3;
		}
	}
}
