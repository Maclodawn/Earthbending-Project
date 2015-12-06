using UnityEngine;
using System.Collections;

public class AIAttackLauncher : AttackLauncher {

	protected static float TIMER_MAX = 1f;
	protected float timer = 1f;

	protected int old_atk = -1;

	protected override void updateInput() {
		if (atk != 0 && timer >= TIMER_MAX) {
			atk = 0;
		}

		if (atk == 0 && atks[0].isFinished() && old_atk == 0)
			atk = -1;

		if (old_atk == 0 && atk != 0)
			timer = 0f;

		if (atk != 0)
			timer += Time.deltaTime;
	}

	public int getAtk() {
		return atk;
	}

	public void Update() {
		updateInput();
		
		if (/*(old_atk >= 0 && atk < 0) || */atk >= atks.Length || isAnyBusy()) return;

		if (old_atk != 0 && atk == 0)
			atks[atk].executeAttack();
		
		//atk = -1;
		old_atk = atk;
	}
}
