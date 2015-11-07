using UnityEngine;
using System.Collections;

public class FlingableFire : MonoBehaviour {

	public Vector3 Direction;
	public float Speed;

	private static CreateFire FireCreator = null;
	private static GameObject Player = null;

	private bool m_canMoveMyself = false;
	private GameObject m_myFirstCollider = null;

	public void Start() {
		if (Player == null)
			Player = GameObject.Find("Player(Clone)");

		if (FireCreator == null)
			FireCreator = GameObject.Find("FireManager").GetComponent<CreateFire>();
	}

	public void Update() {
		if (m_canMoveMyself && m_myFirstCollider == null) {
			transform.position += Direction * Time.deltaTime * Speed;
		} else if (FireCreator.canDetachFire() && !Input.GetKeyDown(KeyCode.P)) {
			FireCreator.reset();
			Direction = Player.transform.forward;
			m_canMoveMyself = true;
		}
	}

	public void OnTriggerEnter(Collider c) {
		Debug.Log(c.gameObject);

		if (m_myFirstCollider == null && !c.gameObject.name.Contains("Terrain") && c.gameObject != Player) {
			m_myFirstCollider = c.gameObject;
		}
	}
}
