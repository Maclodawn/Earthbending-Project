using UnityEngine;
using System.Collections;

public abstract class AttackLauncher : MonoBehaviour {

	protected BasicAttack[] atks;
	protected int atk = -1;

	// map here type of attacks to ids
	public void Start() {
		atks = GetComponent<AttacksGetter>().getOrderedAttacks();
	}

	public void Update() {
		updateInput();
		
		if (atk >= atks.Length || isAnyBusy()) return;
		
		if ((atk == 0 && isKeyDown()) || atk > 0) {
			atks[atk].executeAttack();
		}
	}

	public int getAtk() {
		return atk;
	}

	protected abstract void updateInput();

	public abstract Vector3 getTarget();

	//the key is down since > 1 frame
	public abstract bool isKey();
	
	//the key has just been pushed
	public abstract bool isKeyDown();
	
	//the key has just been released
	public abstract bool isKeyUp();

	protected bool isAnyBusy() {
		foreach (BasicAttack a in atks) {
			if (a.isBusy()) return true;
		}

		return false;
	}
}
