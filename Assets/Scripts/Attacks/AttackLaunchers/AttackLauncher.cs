using UnityEngine;
using System.Collections;

public abstract class AttackLauncher : MonoBehaviour {

	protected BasicAttack[] atks;
	protected int atk = -1;
	protected bool hold, old_hold = false;

	// map here type of attacks to ids
	public void Start() {
		atks = GetComponent<AttacksGetter>().getOrderedAttacks();
	}

	protected abstract void updateInput();

	public void Update() {
		updateInput();

		if (atk < 0 || atk > atks.Length || isBusy()) return;

//		if (atk == 0 || hold) {
//			hold = !atks[0].isThrowable();
//		} else hold = false;

		if (old_hold == false)
			Debug.Log("ok");

		if (atk == 0 && hold && !old_hold)
			atks[0].executeAttack();
		else if (atk != 0)
			atks[atk].executeAttack();

		atk = -1;
		old_hold = hold;
	}

	public bool BasicAtkOnHold() {
		return hold;
	}

	// animation checking
	private bool isBusy() {
		foreach (BasicAttack a in atks) {
			if (a.isBusy()) return true;
		}

		return false;
	}
}
