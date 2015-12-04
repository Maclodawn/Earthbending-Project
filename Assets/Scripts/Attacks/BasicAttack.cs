using UnityEngine;
using System.Collections;

public abstract class BasicAttack : MonoBehaviour {

	protected bool icanexecute;
	protected CharacterMovement executer;

	protected float timer = 0f;

	public void Start() {
		executer = GetComponent<CharacterMovementEarth>();
	}

	public void Update() {
		timer = Mathf.Max(timer+Time.deltaTime, WAIT_TIME());

		if (!icanexecute)
			return;

		updateMe();

		icanexecute = false;
	}

	public void executeAttack() {
		icanexecute = true;
		timer = 0f;
	}

	protected abstract void updateMe();
	protected abstract float WAIT_TIME();
}
