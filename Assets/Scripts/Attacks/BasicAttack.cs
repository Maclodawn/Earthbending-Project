using UnityEngine;
using System.Collections;

public abstract class BasicAttack : MonoBehaviour {

	private bool icanexecute;
	private CharacterMovement executer;

	private float timer = 0f;

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

	public abstract void updateMe();
	public abstract float WAIT_TIME();
}
