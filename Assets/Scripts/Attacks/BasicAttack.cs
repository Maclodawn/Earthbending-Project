using UnityEngine;
using System.Collections;

public abstract class BasicAttack : MonoBehaviour {

	protected bool icanexecute;
	protected float timer = 0f;

	public void executeAttack() {
		icanexecute = true;
	}

	public bool isBusy() {
		return timer < WAIT_TIME();
	}

	public virtual bool isFinished() {
		return true;
	}

	protected abstract void updateMe();
	protected abstract float WAIT_TIME();
}
