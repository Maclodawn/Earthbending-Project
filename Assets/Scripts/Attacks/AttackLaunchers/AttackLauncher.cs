using UnityEngine;
using System.Collections;

public abstract class AttackLauncher : MonoBehaviour {

	protected BasicAttack[] atks;
	protected int atk = -1;
	protected bool hold;

	// map here type of attacks to ids
	public void Start() {
		atks = GetComponent<AttacksGetter>().getOrderedAttacks();
	}

	protected abstract void updateInput();

	public bool BasicAtkOnHold() {
		return atk == 0 && hold;
	}

	protected bool isAnyBusy() {
		foreach (BasicAttack a in atks) {
			if (a.isBusy()) return true;
		}

		return false;
	}
}
