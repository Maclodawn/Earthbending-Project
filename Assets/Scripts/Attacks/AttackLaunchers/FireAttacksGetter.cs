using UnityEngine;
using System.Collections;

public class FireAttacksGetter : AttacksGetter {

	protected override void init() {
		atks = new BasicAttack[0];
		//atks[0] = GetComponent<BasicRockBulletAttack>();
		//atks[1] = GetComponent<>();
		//atks[2] = GetComponent<>();
		//atks[3] = GetComponent<>();
	}
}
