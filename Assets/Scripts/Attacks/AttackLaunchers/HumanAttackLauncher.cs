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

	public override Ray getAimRay() {
		Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
        return ray;
	}

	protected override void updateInput() {
		//cf AttacksGetter (e.g. EarthAttacksGetter) for the input order

		if (atk > 0) atk = -1;

		if (Input.GetButtonDown("Fire1")) {
			atk = 0;
		}

        if (Input.GetButtonDown("Fire2"))
        {
            if (Input.GetAxis("Vertical") < 0)
				atk = 1;
            else if (Input.GetAxis("Vertical") > 0)
				atk = 2;
			else
				atk = 3;
		}
	}
}
