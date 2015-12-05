using UnityEngine;
using System.Collections;

public class AIAttackLauncher : AttackLauncher {

	protected static float HOLD_TIME = 0.4f;
	protected float heldTime = 0f;

	protected override void updateInput() {
		if (hold && heldTime < HOLD_TIME) {
			hold = true;
			heldTime += Time.deltaTime;
		}

		//TODO AI thinking
		atk = 0;

		if (atk == 0) hold = true;
		else hold = false;
	}
}
