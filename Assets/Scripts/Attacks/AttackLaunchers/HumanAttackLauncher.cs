using UnityEngine;
using System.Collections;

public class HumanAttackLauncher : AttackLauncher {

	//the key is down since > 1 frame
	public override bool isKey() {
		return Input.GetButton("Fire1");
	}
	
	//the key has just been pushed
	public override bool isKeyDown() {
		return Input.GetButtonDown("Fire1");
	}
	
	//the key has just been released
	public override bool isKeyUp() {
		return Input.GetButtonUp("Fire1");
	}

	public override Vector3 getTarget() {
		Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width / 2), (Screen.height / 2)));
		RaycastHit hit = new RaycastHit();
		RaycastHit[] hitList = Physics.RaycastAll(ray, 5000);
		
		// Do not take in account this.gameobject as aim
		if (hitList.Length < 1)
			hit.point = ray.direction * 5000;
		else if (!hitList[0].collider.gameObject.name.Contains(gameObject.name))
			hit = hitList[0];
		else if (hitList.Length < 2)
			hit.point = ray.direction * 5000;
		else
			hit = hitList[1];
		
		Vector3 m_forward = hit.point - transform.position;
		m_forward.Normalize();

		return m_forward;
	}

	protected override void updateInput() {
		//cf AttacksGetter (e.g. EarthAttacksGetter) for the input order

		if (atk > 0) atk = -1;

		if (Input.GetButtonDown("Fire1")) {
			atk = 0;
		}
		
		if (Input.GetButtonDown("Fire2")) {
			if (Input.GetKey(KeyCode.S))
				atk = 1;
			else if (Input.GetKey(KeyCode.Z))
				atk = 2;
			else
				atk = 3;
		}
	}
}
