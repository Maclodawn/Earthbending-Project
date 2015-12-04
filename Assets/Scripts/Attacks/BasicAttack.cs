using UnityEngine;
using System.Collections;

public abstract class BasicAttack : MonoBehaviour {

	private bool icanexecute;
	private CharacterMovement executer;
	
	public void Start() {
		executer = GetComponent<CharacterMovementEarth>();
	}

	public void Update() {
		if (!icanexecute)
			return;

		updateMe();

		icanexecute = false;
	}

	public void executeAttack() {
		icanexecute = true;
	}

	public abstract void updateMe();
}
