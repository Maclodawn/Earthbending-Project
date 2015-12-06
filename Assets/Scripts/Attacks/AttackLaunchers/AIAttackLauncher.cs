﻿using UnityEngine;
using System.Collections;

public class AIAttackLauncher : AttackLauncher {

	protected static float TIMER_MAX = 1f;
	protected float timer = 1f;
	protected bool key = false, keyUp = false, keyDown = false;

	// ---

	protected bool clic() {
		if (key || keyDown || keyDown) return false;

		key = false;
		keyUp = false;
		keyDown = true;

		return true;
	}

	protected bool cliclic() {
		if (!keyDown || keyUp) return false;

		key = true;
		keyUp = false;
		keyDown = false;

		return true;
	}

	protected bool clac() {
		if (!key || keyUp) return false;
		
		key = false;
		keyUp = true;
		keyDown = false;

		return true;
	}

	protected bool claclac() {
		if (!keyUp || key) return false;

		key = false;
		keyUp = false;
		keyDown = false;

		return true;
	}

	// ---

	//the key is down since > 1 frame
	public override bool isKey() {
		return key;
	}

	//the key has just been pushed
	public override bool isKeyDown() {
		return keyDown;
	}

	//the key has just been released
	public override bool isKeyUp() {
		return keyUp;
	}

	public int getAtk() {
		return atk;
	}

	// ---
	
	protected override void updateInput() {
		//AI thinking here
		int tmp_atk = 0;

		if (tmp_atk == 0) {
			if (keyDown)
				cliclic();

			if ((!key || keyUp) && timer >= TIMER_MAX) {
				clic();
				atk = 0;
			}

			if (atks[0].isFinished() && key) {
				atk = -1;
				clac();
			}
			
			if (keyUp) {
				claclac();
				timer = 0f;
			}

			if (keyUp || !key)
				timer += Time.deltaTime;
		} else {
			atk = tmp_atk;
		}
	}
}
