using UnityEngine;
using System.Collections;

public class CreateFire : MonoBehaviour {

	//to create fire, the player need to "charge" power during at least two seconds

	public GameObject FirePrefab;
	public Vector3 OffsetToPlayer;

	private float TIME_TO_CHARGE = 2.0f; //don't forget to change delta time after start in particle system, if any change here
	private GameObject PLAYER;
	private bool KeyDownP = false;

	private float m_currentElapsedTime = 0.0f;
	private GameObject m_currentNewFire = null;
	private bool m_canDetachFire = false;

	public void Start() {
		PLAYER = GameObject.Find("Player(Clone)"); //TODO Is there not another better way?
	}

	public void Update() {
		Debug.Log(Input.GetKeyDown(KeyCode.P));

		if (m_canDetachFire)
			return;

		if (Input.GetKeyUp(KeyCode.P))
			KeyDownP = false;

		if (KeyDownP || Input.GetKeyDown(KeyCode.P)) { //if (!Input.GetKeyUp(KeyCode.P)) {
			KeyDownP = true;

			if (m_currentNewFire == null)
				m_currentNewFire = Instantiate(FirePrefab);

			m_currentElapsedTime += Time.deltaTime;
			m_currentNewFire.transform.position = PLAYER.transform.position;
		} else if (m_currentElapsedTime < TIME_TO_CHARGE) {
				GameObject.Destroy(m_currentNewFire);
				m_currentNewFire = null;
				m_currentElapsedTime = 0.0f;
				//KeyDownP = false;
		} else m_canDetachFire = true;
	}

	public bool canDetachFire() {
		return m_canDetachFire;
	}

	public void reset() {
		m_currentElapsedTime = 0.0f;
		m_canDetachFire = false;
		m_currentNewFire = null;
	}
}
