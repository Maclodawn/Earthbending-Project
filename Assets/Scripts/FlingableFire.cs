using UnityEngine;
using System.Collections;

public class FlingableFire : MonoBehaviour {

	public Vector3 Direction;
	public float Speed;

	private static CreateFire FireCreator = null;
	private static GameObject Player = null;

	private bool m_canMoveMyself = false;
	private GameObject m_myFirstCollider = null;
	private Collider m_myCollider;

	private static bool Destroying = false;

	public void Start() {
		if (Player == null)
			Player = GameObject.Find("Player(Clone)");

		if (FireCreator == null)
			FireCreator = GameObject.Find("FireManager").GetComponent<CreateFire>();
	}

	public void Update() {
		if (Destroying)
			Destroy(this.gameObject);

		if (m_myFirstCollider != null) {
			//gameObject.GetComponentInChildren<ParticleSystem>().transform.localScale *= 1.2f;
			transform.localScale *= 1.2f;
			//gameObject.GetComponentInChildren<ParticleSystem>().emissionRate *= 1.2f;
			gameObject.GetComponentInChildren<ParticleSystem>().startSize *= 1.2f;
			transform.position = new Vector3(transform.position.x, transform.position.y+1.4f, transform.position.z);

			if (m_myCollider == null)
				m_myCollider = gameObject.GetComponent<Collider>();

			Collider myFirstCollider = m_myFirstCollider.gameObject.GetComponent<Collider>();
			float myCurrentVolume = m_myCollider.bounds.size.x * m_myCollider.bounds.size.y * m_myCollider.bounds.size.z;
			float myFirstColliderVolume = myFirstCollider.bounds.size.x * myFirstCollider.bounds.size.y * myFirstCollider.bounds.size.z;

			if (myCurrentVolume > myFirstColliderVolume) {
				Destroy(m_myFirstCollider.gameObject);
				//Destroy(gameObject);
			}
		}

		if (m_canMoveMyself && m_myFirstCollider == null) {
			transform.position += Direction * Time.deltaTime * Speed;
		} else if (FireCreator.canDetachFire() && !Input.GetKeyDown(KeyCode.P)) {
			FireCreator.reset();
			Direction = Player.transform.forward;
			m_canMoveMyself = true;
		}
	}

	public void OnCollisionEnter(Collision c) {
		if (m_myFirstCollider == null && !(c.collider.gameObject.name.Contains("Terrain") || c.collider.gameObject.name.Contains("Fire")) && c.collider.gameObject != Player) {
			m_myFirstCollider = c.collider.gameObject;
		}
	}
}
