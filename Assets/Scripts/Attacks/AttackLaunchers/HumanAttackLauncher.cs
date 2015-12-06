using UnityEngine;
using System.Collections;

public class HumanAttackLauncher : AttackLauncher {

	//the key is down since > 1 frame
	public override bool isKey() {
		return Input.GetButton("Fire1");
	}
	
	//the key has just been pushed
	public override bool isKeyDown() {
		return Input.GetButtonDown("Fire1");
	}
	
	//the key has just been released
	public override bool isKeyUp() {
		return Input.GetButtonUp("Fire1");
	}

	protected override void updateInput() {
		if (Input.GetButtonDown("Fire1")) {
			atk = 0;
		}
		
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
