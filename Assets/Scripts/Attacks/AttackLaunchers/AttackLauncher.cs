using UnityEngine;
using System.Collections;

public abstract class AttackLauncher : MonoBehaviour {

	private BasicAttack[] atks;
	protected int atk = -1;
	protected bool hold;

	// map here type of attacks to ids
	public void Start() {
		atks = GetComponent<AttacksGetter>().getOrderedAttacks();
	}

	protected abstract void updateInput();

	public void Update() {
		updateInput();

		if (atk < 0 || atk > atks.Length || isBusy()) return;

		atks[atk].executeAttack();

		atk = -1;
	}

	public bool BasicAtkOnHold() {
		return atk == 0 && hold;
	}

	private bool isBusy() {
		foreach (BasicAttack a in atks) {
			if (a.isBusy()) return true;
		}

		return false;
	}
}
