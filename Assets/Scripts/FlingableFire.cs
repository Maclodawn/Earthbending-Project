using UnityEngine;
using System.Collections;

public class FlingableFire : MonoBehaviour {

	public Vector3 Direction;
	public float Speed;

	private static CreateFire FireCreator = null;

	private bool m_canMoveMyself = false;

	public void Start() {
		if (FireCreator == null)
			FireCreator = GameObject.Find("FireManager").GetComponent<CreateFire>();
	}

	public void Update() {
		if (m_canMoveMyself) {
			transform.position += Direction * Time.deltaTime * Speed;
		} else if (FireCreator.canDetachFire() && !Input.GetKeyDown(KeyCode.P)) {
			FireCreator.reset();
			m_canMoveMyself = true;
		}
	}
}
