using UnityEngine;
using System.Collections;

public abstract class BasicAttack : MonoBehaviour {

	protected bool icanexecute;
	protected CharacterBasic executer;

	protected float timer = 0f;

	public void Start() {
		executer = GetComponent<CharacterBasic>();
	}

	public void Update() {
		if (timer < WAIT_TIME())
			timer += Time.deltaTime;

		if (!icanexecute || timer < WAIT_TIME())
			return;

		updateMe();

		icanexecute = false;
		timer = 0f;
	}

	public void executeAttack() {
		icanexecute = true;
	}

	protected abstract void updateMe();
	protected abstract float WAIT_TIME();
}
