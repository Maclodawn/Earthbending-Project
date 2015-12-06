using UnityEngine;
using System.Collections;

public class AIAttackLauncher : AttackLauncher {
	
	protected override void updateInput() {
		//TODO AI thinking here
		atk = 0; //(Random.Range(0, 11) > 9) ? 0 : -1;

		if (hold) {
			bool tmp = atks[0].isFinished();
			if (tmp)
				hold = false;
		} else if (atk == 0)
			hold = true;
	}
}
